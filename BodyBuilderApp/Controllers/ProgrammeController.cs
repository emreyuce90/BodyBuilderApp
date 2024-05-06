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
    }
}
