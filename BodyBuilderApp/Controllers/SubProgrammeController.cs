using BodyBuilder.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SubProgrammeController : ControllerBase {
        private readonly ISubProgrammeService _subProgrammeService;

        public SubProgrammeController(ISubProgrammeService subProgrammeService) {
            _subProgrammeService = subProgrammeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByrogrammeId([FromRoute] Guid id) {
            return Ok(await _subProgrammeService.GetByIdAsync(id));
        }
    }
}
