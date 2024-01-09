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

namespace BodyBuilder.Application.Utilities.JWT {
    public class TokenCreate : ITokenCreate {
        private readonly IConfiguration _configuration;

        public TokenCreate(IConfiguration configuration) {
            _configuration = configuration;
        }

        public AccessToken CreateToken(User user) {

            var tokenExpireDate = DateTime.Now.AddHours(Convert.ToInt32(_configuration["TokenOptions:AccessTokenExpiration"]));
            var refreshTokenExpireDate = DateTime.Now.AddDays(Convert.ToInt32(_configuration["TokenOptions:ExpireRefresh"]));
            SymmetricSecurityKey symmetric = new(Encoding.UTF8.GetBytes(_configuration["TokenOptions:SecurityKey"]));
            SigningCredentials credentials = new SigningCredentials(symmetric, SecurityAlgorithms.HmacSha256);

            //claims
            var claims = new List<Claim>() {
            new Claim("Id", $"{user.Id}"),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, $"{user.Role} ")
            };

            //token create
            JwtSecurityToken securityToken = new(
                issuer: _configuration["TokenOptions:Issuer"],
                audience: _configuration["TokenOptions:Audience"],
                expires: tokenExpireDate,
                notBefore: DateTime.Now,
                signingCredentials: credentials,
                claims: claims

                );

            JwtSecurityTokenHandler tokenHandler = new();
            var accessToken = new AccessToken {
                ExpirationDate = tokenExpireDate,
                Token = tokenHandler.WriteToken(securityToken),
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpireDate
            };
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
