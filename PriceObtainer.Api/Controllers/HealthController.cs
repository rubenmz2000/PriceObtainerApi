using Microsoft.AspNetCore.Mvc;

namespace PriceObtainer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok("Healthy");
    }
}