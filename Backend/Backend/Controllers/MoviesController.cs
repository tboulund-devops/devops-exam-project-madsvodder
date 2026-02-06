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
    
    [HttpGet]
    public IEnumerable<IActionResult> GetSpecific()
    {
        return null;
    }
    
    [HttpGet]
    public IEnumerable<IActionResult> GetTop10()
    {
        return null;
    }
    
    [HttpGet]
    public IEnumerable<IActionResult> GetTop5()
    {
        return null;
    }
    
    [HttpGet]
    public IEnumerable<IActionResult> GetTop()
    {
        return null;
    }
}