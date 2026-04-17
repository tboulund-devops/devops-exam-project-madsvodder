using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(MovieService movieService) : ControllerBase
{
    
    // GET/api/movies/ - Gets all movies
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await movieService.GetAllAsync();
        return Ok(movies);
    }
    
    // Gets top 5
    [HttpGet("top")]
    public async Task<IActionResult> GetTop()
    {
        var movies = await movieService.GetTop();
        return Ok(movies);
    }
    
    
    //GET /api/movies/8
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSpecific(int id)
    {
        var movie = await movieService.GetByIdAsync(id);
        if (movie is null) return NotFound();
        return Ok(movie);
    }
    
    // POST /api/movies
    [HttpPost]
    public async Task<IActionResult> Create(Movie movie)
    {
        var newMovie = await movieService.CreateAsync(movie);
        return CreatedAtAction(nameof(GetSpecific), new { id = newMovie.Id }, newMovie);
    }
    
    //PUT /api/movies/23
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Movie movie)
    {
        if (id != movie.Id)
            return BadRequest("Movie not found. Wrong ID?");

        var success = await movieService.UpdateAsync(id, movie);
        if (!success)
            return NotFound();

        return NoContent();
    }
    
    //DELETE /api/movies/45
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await movieService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}