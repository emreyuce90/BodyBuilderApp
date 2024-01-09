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

        [HttpPost]
        [ProducesResponseType(typeof(Response<UserDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> SaveAsync(UserAddDto userAddDto) {
            //check email already exist
            var user = await _userService.GetUserByEMail(userAddDto.Email);
            if (user != null) {
                return BadRequest(new Response() { Message="Bu mail adresi zaten veritabanımızda mevcuttur",Success=false});

            }
            //validate user dto
            var validationResult = await _userValidator.ValidateAsync(userAddDto);
            if (!validationResult.IsValid) {
                foreach (var item in validationResult.Errors) {
                    //_logger.LogError(item.ErrorMessage, item.ErrorCode);
                }
                return BadRequest(new Response(validationResult.Errors));
            }
            var userDto = await _userService.AddAsync(userAddDto);
            return Ok(userDto);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<UserDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(Guid id) {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        
    }
}
