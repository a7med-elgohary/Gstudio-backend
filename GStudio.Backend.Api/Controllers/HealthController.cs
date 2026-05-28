using Microsoft.AspNetCore.Mvc;

namespace GStudio.Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Lightweight keepalive endpoint — used by UptimeRobot to prevent IIS cold starts.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "ok", timestamp = DateTime.UtcNow });
        }
    }
}
