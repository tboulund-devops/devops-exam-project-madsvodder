using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class MovieService
{
    private readonly MovieContext _context;

    public MovieService(MovieContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetAllAsync()
    {
        return await _context.Movies
            .AsNoTracking()
            .OrderBy(m => m.Id)
            .ToListAsync();
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        movie.Id = 0;

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task<bool> UpdateAsync(int id, Movie updated)
    {
        var oldMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (oldMovie is null) return false;

        oldMovie.Title = updated.Title;
        oldMovie.Year = updated.Year;
        oldMovie.Rating = updated.Rating;
        oldMovie.Description = updated.Description;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (entity is null) return false;

        _context.Movies.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}