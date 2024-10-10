using AutoMapper;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilderApp.Communication;
using BodyBuilderApp.Models;
using BodyBuilderApp.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public partial class AuthController : ControllerBase {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, IMapper mapper) {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<UserResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto) {
            var response = await _authService.VerifyUser(userLoginDto);
            if (!response.Success) {
                return BadRequest(response);
            }
            var token = await _authService.CreateToken(userLoginDto);
            
            return Ok(new Response<UserResource>(token));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Register (UserAddDto userAddDto) {
            var userDto = await _authService.RegisterUser(userAddDto);
            return Ok(userDto);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Response<UserResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTokenByRefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest) {
            //var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshTokenRequest.RefreshToken)) {
                // refreshToken bulunamazsa veya boşsa, hata işlemleri yapabilirsiniz
                return BadRequest("Invalid or missing refresh token.");
            }
            var response = await _authService.CreateTokenByRefreshToken(refreshTokenRequest.RefreshToken);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync() {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) {
                return BadRequest("Invalid or missing refresh token.");
            }
            var response = await _authService.CleanRefreshToken(refreshToken);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
}
