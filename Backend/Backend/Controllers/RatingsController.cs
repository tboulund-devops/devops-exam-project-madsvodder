using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/movies/{movieId:int}/ratings")]
public class RatingsController(RatingService ratingService) : ControllerBase
{
    
    //GET /api/movies/3/ratings
    [HttpGet]
    public async Task<IActionResult> GetAll(int movieId)
    {
        var ratings = await ratingService.GetByMovieIdAsync(movieId);
        
        return Ok(ratings);
    }
    
    //GET /api/movies/5/ratings/average
    [HttpGet("average")]
    public async Task<IActionResult> GetAverage(int movieId)
    {
        var average = await ratingService.GetAverageScoreAsync(movieId);
        
        if (average is null) return NotFound("No ratings was found for this movie");
        
        return Ok(new { average });
    }
    
    //POST /api/movies/7/ratings
    [HttpPost]
    public async Task<IActionResult> Create(int movieId, Rating rating)
    {
        rating.MovieId = movieId;
        var created = await ratingService.CreateAsync(rating);
        
        if (created is null) return NotFound("Film not found");

        return CreatedAtAction(nameof(GetAll), new { movieId }, created);
    }
    
    //DELETE /api/movies/8/ratings/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int movieId, int id)
    {
        var deleted = await ratingService.DeleteAsync(movieId, id);
        
        if (!deleted) return NotFound();
        
        return NoContent();
    }
    
}