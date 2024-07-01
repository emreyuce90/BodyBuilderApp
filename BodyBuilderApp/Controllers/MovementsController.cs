using BodyBuilder.Application.Dtos.Movement;
using BodyBuilder.Application.Dtos.User;
using BodyBuilder.Application.Interfaces;
using BodyBuilderApp.Communication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MovementsController : ControllerBase {
        private readonly IMovementService _movementService;

        public MovementsController(IMovementService movementService) {
            _movementService = movementService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(Response<List<MovementDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll() {
            return Ok(new Response<List<MovementDto>>(await _movementService.GetAllAsync()));

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<MovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            return Ok(await _movementService.GetByIdAsync(id));

        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<MovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMovement([FromBody] MovementAddDto movementAddDto) {
            return Ok(await _movementService.AddAsync(movementAddDto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response<MovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMovement([FromBody] MovementUpdateDto movementUpdateDto) {

            return Ok(await _movementService.UpdateAsync(movementUpdateDto));
        }
        
       
        [HttpGet("bodypart/{bodypartId}")]
        public async Task<IActionResult> GetMovementsByBodyPartId(Guid bodypartId) {
            return Ok(await _movementService.GetMovementByBodypartId(bodypartId));
        }

    }
}