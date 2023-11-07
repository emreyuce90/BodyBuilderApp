using BodyBuilder.Application.Interfaces;
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

        public AuthService(IUserRepository userRepository) {
            _userRepository = userRepository;
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
