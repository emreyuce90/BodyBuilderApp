using BodyBuilder.Application.Dtos.Bodypart;
using BodyBuilder.Application.Dtos.Movement;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Application.Services;
using BodyBuilderApp.Communication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BodypartController : ControllerBase {
        private readonly IBodyPartService _bodyPartService;

        public BodypartController(IBodyPartService bodyPartService) {
            _bodyPartService = bodyPartService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<List<BodyPartDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll() {
            return Ok(await _bodyPartService.GetAllAsync());

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<BodyPartDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            return Ok(await _bodyPartService.GetByIdAsync(id));

        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<BodyPartDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMovement([FromBody] BodyPartAddDto bodypartAddDto) {
            return Ok(await _bodyPartService.AddAsync(bodypartAddDto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response<BodyPartDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] BodyPartUpdateDto bodyPartUpdateDto) {
            return Ok(await _bodyPartService.UpdateAsync(bodyPartUpdateDto));
        }
    }
}
