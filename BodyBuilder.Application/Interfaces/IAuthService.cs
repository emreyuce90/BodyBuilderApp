using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Utilities.JWT;
using BodyBuilderApp.Communication;
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
        Task<Response> VerifyUser(UserLoginDto userLoginDto);
        Task<Response> CreateToken(UserLoginDto userLoginDto);
        Task<Response> RegisterUser(UserAddDto userAddDto);
        Task<Response> CreateTokenByRefreshToken(string refreshToken);
    }
}
