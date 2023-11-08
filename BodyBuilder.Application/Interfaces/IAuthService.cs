using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Utilities.JWT;
using BodyBuilderApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> VerifyUser(UserLoginDto userLoginDto);
        AccessToken CreateToken(UserDto userDto);
    }
}
