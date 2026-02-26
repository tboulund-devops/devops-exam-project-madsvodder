using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class RatingService
{
    private readonly AppDbContext _context;
    
    public RatingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rating>> GetByMovieIdAsync(int movieId)
    {
        return await _context.Ratings
            .Where(r => r.MovieId == movieId)
            .ToListAsync();
    }

    public async Task<Rating?> GetByIdAsync(int id)
    {
        return await _context.Ratings.FindAsync(id);
    }

    public async Task<Rating?> CreateAsync(Rating rating)
    {
        var movieExists = await _context.Movies.AnyAsync(m => m.Id == rating.MovieId);
        if (!movieExists) return null;

        _context.Ratings.Add(rating);
        
        await _context.SaveChangesAsync();
        
        return rating;
    }

    public async Task<bool> DeleteAsync(int movieId, int ratingId)
    {
        var rating = await _context.Ratings
            .FirstOrDefaultAsync(r => r.Id == ratingId && r.MovieId == movieId);

        if (rating is null) return false;

        _context.Ratings.Remove(rating);
        
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<double?> GetAverageScoreAsync(int movieId)
    {
        return await _context.Ratings
            .Where(r => r.MovieId == movieId)
            .Select(r => (double?)r.Score)
            .AverageAsync();
    }
}