using BodyBuilder.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProgrammeController : ControllerBase {
        private readonly IProgrammeService _programmeService;

        public ProgrammeController(IProgrammeService programmeService) {
            _programmeService = programmeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() {

            return Ok(await _programmeService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllByUserId(Guid id) {
            return Ok(await _programmeService.GetByUserIdAsync(id));
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateProgramme([FromRoute] Guid userId, [FromBody] CreateCustomProgramme createCustomProgramme) {
            return Ok(await _programmeService.CreateCustomWorkoutAsync(userId, createCustomProgramme));
        }
        /// <summary>
        /// Guid tipinde programmeId alır ve programı veritabanından soft delete olarak siler
        /// </summary>
        /// <param name="programmeId"></param>
        /// <returns>Geriye success ya da errormessage döner</returns>
        [HttpDelete("{programmeId}")]
        public async Task<IActionResult> DeleteProgramme([FromRoute]Guid programmeId) {
            return Ok(await _programmeService.DeleteProgrammeAsync(programmeId));
        }

    }
}
