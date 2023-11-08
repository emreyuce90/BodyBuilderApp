using AutoMapper;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Application.Utilities.JWT;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Domain.Utilities;
using BodyBuilderApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {
    public class AuthService : IAuthService {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, ITokenHelper tokenHelper, IMapper mapper) {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
        }

        public AccessToken CreateToken(UserDto userDto) {
            var accessToken = _tokenHelper.CreateToken(_mapper.Map<User>(userDto), null);
            return accessToken;
        }

        public async Task<bool> VerifyUser(UserLoginDto userLoginDto) {
            var user = await _userRepository.GetSingle(u => u.Email == userLoginDto.EMail);
            if (user == null) {
                return false;
            }
            bool isTrue = HashingHelper.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);
            return isTrue;

        }


    }
}
