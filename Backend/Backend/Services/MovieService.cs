using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class MovieService(AppDbContext context)
{
    public async Task<List<Movie>> GetAllAsync()
    {
        return await context.Movies
            .AsNoTracking()
            .OrderBy(m => m.Id)
            .ToListAsync();
    }

    public async Task<Movie> GetSecondAsync()
    {
        return await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == 1);
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        movie.Id = 0;

        context.Movies.Add(movie);
        await context.SaveChangesAsync();
        return movie;
    }

    public async Task<bool> UpdateAsync(int id, Movie updated)
    {
        var oldMovie = await context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (oldMovie is null) return false;

        oldMovie.Title = updated.Title;
        oldMovie.Year = updated.Year;
        oldMovie.Description = updated.Description;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (entity is null) return false;

        context.Movies.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}