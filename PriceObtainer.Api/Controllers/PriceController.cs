using Microsoft.AspNetCore.Mvc;
using PriceObtainerApi.Services;
namespace PriceObtainerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriceController : ControllerBase
{
    private readonly PriceService _priceService = new();
    
    [HttpGet]
    public IActionResult GetPrice()
    {
        
        return Ok(_priceService.GetPrice());
    }
}