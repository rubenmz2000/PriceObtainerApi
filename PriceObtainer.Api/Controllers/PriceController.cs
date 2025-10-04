using Microsoft.AspNetCore.Mvc;
using PriceObtainer.Library.Services;
namespace PriceObtainer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriceController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPrice(string symbol, [FromServices] PriceService priceService)
    {
        var result = await priceService.GetPriceAsync(symbol);
        
        return Ok(result);
    }
}