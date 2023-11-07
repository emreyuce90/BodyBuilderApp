using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilderApp.Communication;
using BodyBuilderApp.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BodyBuilderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<UserResource>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto) {
            bool isOk = await _authService.VerifyUser(userLoginDto);
            if (!isOk) {
                return BadRequest(new Response() { Message="Kullanıcı adı veya şifre hatalıdır",Success = false});
            }
            //JWT Create
            return Ok();
        }
    }
}
