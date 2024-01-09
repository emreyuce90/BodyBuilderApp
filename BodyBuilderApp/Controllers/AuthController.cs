using AutoMapper;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilderApp.Communication;
using BodyBuilderApp.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, IUserService userService, IMapper mapper) {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<UserResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto) {
            var response = await _authService.VerifyUser(userLoginDto);
            if (!response.Success) {
                return BadRequest(new Response() { Message = "Kullanıcı adı veya şifre hatalıdır", Success = false });
            }
            var token = await _authService.CreateToken(userLoginDto);
            
            return Ok(new Response<UserResource>(token));
        }
    }
}
