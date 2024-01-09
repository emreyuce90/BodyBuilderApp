using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilderApp.Communication;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly IUserService _userService;
        private readonly IValidator<UserAddDto> _userValidator;

        public UserController(IUserService userService, IValidator<UserAddDto> userValidator) {
            _userService = userService;
            _userValidator = userValidator;
        }

        [HttpGet]

        [ProducesResponseType(typeof(Response<List<UserDto>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> Get() {

            return Ok(await _userService.GetAllAsync(false));
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<UserDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(Guid id) {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        
    }
}
