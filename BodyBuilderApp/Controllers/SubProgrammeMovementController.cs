using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SubProgrammeMovementController : ControllerBase {
        private readonly ISubProgrammeMovementService _subProgrammeMovementService;

        public SubProgrammeMovementController(ISubProgrammeMovementService subProgrammeMovementService) {
            _subProgrammeMovementService = subProgrammeMovementService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBySubProgrammeId(Guid id) {
            return Ok(await _subProgrammeMovementService.GetAllBySubProgrammeIdAsync(id));

        }

        [HttpPut("{subProgrammeId}")]
        public async Task<IActionResult> UpdateSubProgrammeMovements([FromRoute]Guid subProgrammeId,[FromBody]List<SubProgrammeMovement> programmeMovements) {
            return Ok(await _subProgrammeMovementService.UpdateSubProgrammeMovementByIdAsync(subProgrammeId, programmeMovements));
        }
    }
}
