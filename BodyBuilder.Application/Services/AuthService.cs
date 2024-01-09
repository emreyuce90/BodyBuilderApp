using AutoMapper;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Application.Utilities.JWT;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Domain.Utilities;
using BodyBuilderApp.Communication;
using BodyBuilderApp.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {
    public class AuthService : IAuthService {
        private readonly IUserRepository _userRepository;
        private readonly ITokenCreate _tokenCreate;
        private readonly IUserRefreshToken _userRefreshToken;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, ITokenCreate tokenCreate, IMapper mapper, IUserRefreshToken userRefreshToken) {
            _userRepository = userRepository;
            _tokenCreate = tokenCreate;
            _mapper = mapper;
            _userRefreshToken = userRefreshToken;
        }

        public async Task<Response> CreateToken(UserLoginDto userLoginDto) {
            //check user existence
            var user = await _userRepository.Table.Include(r=>r.Role).FirstOrDefaultAsync(u => u.Email == userLoginDto.EMail);
            if (user == null) return new Response("Böyle bir kullanıcı bulunamadı");
            //Eğer user var ise ve silinmemiş ise
            if (user != null && !user.IsDeleted) {
                //access token üret
                var accessToken = _tokenCreate.CreateToken(user);
                //accessToken üretildiğinde refresh token da üretilmiş olur
                //kullanıcının refresh token ı yok ise bu üreilen refresh token ı refresh token tablosuna yaz
                //eğer kullanıcının refresh tokenı var ise tablodaki refresh tokenı güncelle
                var refreshToken = await _userRefreshToken.Table.FirstOrDefaultAsync(r => r.UserId == user.Id);

                if (refreshToken == null) {
                    await _userRefreshToken.CreateAsync(new UserRefreshToken { UserId = user.Id, Code = accessToken.RefreshToken, Expiration = accessToken.RefreshTokenExpiration });
                } else {
                    refreshToken.Code = accessToken.RefreshToken;
                    refreshToken.Expiration = accessToken.RefreshTokenExpiration;
                    _userRefreshToken.UpdateAsync(refreshToken);
                }
                await _userRefreshToken.SaveAsync();

                var userResource = new UserResource {
                    Email=user.Email,
                    Id=user.Id,
                    RefreshToken = accessToken.RefreshToken,
                    RoleName= user.Role.ToString(),
                    Token=accessToken.Token,
                    
                };
                return new Response {
                    Success = true,
                    Code = 200,
                    Resource = userResource
                };
            }

            return new Response("Kullanıcı silinmiş veya aktif değil");
        }

        public async Task<Response> VerifyUser(UserLoginDto userLoginDto) {
            var user = await _userRepository.GetSingle(u => u.Email == userLoginDto.EMail);
            if (user == null) {
                return new Response("Kullanıcı adı veya şifre hatalıdır");
            }
            bool isTrue = HashingHelper.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);
            return new Response();

        }



    }



}

