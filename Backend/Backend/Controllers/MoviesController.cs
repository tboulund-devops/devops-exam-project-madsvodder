using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MovieService _movies;

    public MoviesController(MovieService movies)
    {
        _movies = movies;
    }
    
    // GET/api/movies/ - Gets all movies
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies= await _movies.GetAllAsync();
        return Ok(movies);
    }
    
    //GET /api/movies/8
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSpecific(int id)
    {
        var movie = await _movies.GetByIdAsync(id);
        if (movie is null) return NotFound();
        return Ok(movie);
    }
    
    // POST /api/movies
    [HttpPost]
    public async Task<IActionResult> Create(Movie movie)
    {
        var newMovie = await _movies.CreateAsync(movie);
        return CreatedAtAction(nameof(GetSpecific), new { id = newMovie.Id }, newMovie);
    }
    
    //PUT /api/movies/23
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Movie movie)
    {
        if (id != movie.Id)
            return BadRequest();

        var success = await _movies.UpdateAsync(id, movie);
        if (!success)
            return NotFound();

        return NoContent();
    }
    
    //DELETE /api/movies/45
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _movies.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
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