using Azure;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync() {
            return Ok(await _userService.GetAllAsync());  
        }
    }
}
