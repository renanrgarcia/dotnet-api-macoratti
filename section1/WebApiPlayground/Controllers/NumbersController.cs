using Microsoft.AspNetCore.Mvc;
using WebApiPlayground.Services;

namespace WebApiPlayground.Controllers;

[ApiController]
[Route("[controller]")]
public class NumbersController(NumbersService _numbersService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetNumber()
    {
        return Ok(_numbersService.GetNumber());
    }
}
