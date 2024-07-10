using BodyBuilder.Application.Interfaces;
using BodyBuilderApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : ControllerBase {
        private readonly IMetricsService _metricsService;

        public MetricsController(IMetricsService metricsService) {
            _metricsService = metricsService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUsersMetrics([FromRoute] Guid userId) {
            return Ok(await _metricsService.GetUsersMetrics(userId));

        }

        [HttpPost]
        public async Task<IActionResult> CreateUserMetrics([FromBody]CreateUserMetricDto createUserMetricDto) {
            return Ok(await _metricsService.CreateUserMetricsAsync(createUserMetricDto.UserId,createUserMetricDto.Value,createUserMetricDto.BodyMetricId));
        }

        [HttpGet("{userId}/{bodymetricId}")]
        public async Task<IActionResult> GetUserMetricLogs([FromRoute]Guid userId, [FromRoute] Guid bodymetricId) {
            return Ok(await _metricsService.GetUserMetricLogsAsync(userId, bodymetricId));
        }
    }
}
