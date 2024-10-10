using BodyBuilder.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BodyBuilder.Application.Utilities.JWT {
    public class TokenCreate : ITokenCreate {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenCreate(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public AccessToken CreateToken(User user) {

            var tokenExpireDate = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["TokenOptions:AccessTokenExpiration"]));
            var refreshTokenExpireDate = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["TokenOptions:ExpireRefresh"]));
            SymmetricSecurityKey symmetric = new(Encoding.UTF8.GetBytes(_configuration["TokenOptions:SecurityKey"]));
            SigningCredentials credentials = new SigningCredentials(symmetric, SecurityAlgorithms.HmacSha256);

            //claims
            var claims = new List<Claim>() {
            new Claim("Id", $"{user.Id}"),
            new Claim(ClaimTypes.Email, user.Email),
            
            };

            //userRoles
            foreach (var r in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,r.RoleName));
            }

            //token create
            JwtSecurityToken securityToken = new(
                issuer: _configuration["TokenOptions:Issuer"],
                audience: _configuration["TokenOptions:Audience"],
                expires: tokenExpireDate,
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials,
                claims: claims

                );

            JwtSecurityTokenHandler tokenHandler = new();
            var accessToken = new AccessToken {
                ExpirationDate = tokenExpireDate,
                Token = tokenHandler.WriteToken(securityToken),
                // RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpireDate
            };
            //refresh token oluştur
            accessToken.RefreshToken = CreateRefreshToken();
            //jwt üzerinde cookie oluştur
            var response = _httpContextAccessor.HttpContext.Response;
            response.Cookies.Append("refreshToken",accessToken.RefreshToken,new CookieOptions {
                HttpOnly=true,
                Secure=true,
                SameSite = SameSiteMode.None
            });

            return accessToken;
        }

        private string CreateRefreshToken() {
            var numberByte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
    }
}
