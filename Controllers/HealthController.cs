using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        // Allow anonymous requests so health probes do not require authentication
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            // Return a simple JSON indicating service health status
            return Ok(new { status = "Healthy" });
        }
    }
}