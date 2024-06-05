using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Album.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        private readonly HealthCheckService _service;

        public HealthController(ILogger<HealthController> logger, HealthCheckService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var report = await _service.CheckHealthAsync();

            _logger.LogInformation($"Get Health Information: {report}");

            return report.Status == HealthStatus.Healthy ? Ok(report) : StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
        }
    }
}
