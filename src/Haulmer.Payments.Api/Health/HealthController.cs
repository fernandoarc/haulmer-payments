using Microsoft.AspNetCore.Mvc;

namespace Haulmer.Payments.Api.Health;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { service = "Haulmer.Payments.Api", status = "Healthy" });
    }
}
