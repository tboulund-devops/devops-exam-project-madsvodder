using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{

    [HttpGet]
    public IEnumerable<IActionResult> GetAll()
    {
        return null;
    }
}