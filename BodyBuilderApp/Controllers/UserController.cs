﻿using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilderApp.Communication;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers
{
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

            return Ok(new Response<List<UserDto>>(await _userService.GetAllAsync(false)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<UserDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> SaveAsync(UserAddDto userAddDto) {
            //validate user dto
            var validationResult = await _userValidator.ValidateAsync(userAddDto);
            if (!validationResult.IsValid) {
                foreach (var item in validationResult.Errors) {
                    //_logger.LogError(item.ErrorMessage, item.ErrorCode);
                }
                return BadRequest(new Response(validationResult.Errors));
            }
            var userDto = await _userService.AddAsync(userAddDto);
            return Ok(new Response<UserDto>(userDto));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<UserDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(Guid id) {
            var user = await _userService.GetById(id);
            return Ok(new Response<UserDto>(user));
        }

        
    }
}
