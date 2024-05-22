using Album.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Album.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : Controller
    {

        private readonly ILogger<HelloController> _logger;
        private readonly GreetingService gs;

        public HelloController(GreetingService greetingService, ILogger<HelloController> logger)
        {
            _logger = logger;
            gs = greetingService;

        }
        [HttpGet("")]
        public async Task<IActionResult>Hello(string name)
        {
            return Ok(gs.greeting(name));
        }
        
        
    }
}
