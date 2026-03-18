using System.Text;
using Backend;
using Backend.Data;
using Backend.Entities;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using Assert = Xunit.Assert;

namespace FreshTomatoes.Tests;

public class UnitTest1
{
    private static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new AppDbContext(options);
    }

    private static IConfiguration CreateConfig() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "super-secret-test-key-super-secret-test-key",
                ["Jwt:Issuer"] = "test-issuer",
                ["Jwt:Audience"] = "test-audience"
            })
            .Build();

    [Fact]
    public async Task MovieService_CreateAsync_SavesMovie()
    {
        await using var context = CreateContext(nameof(MovieService_CreateAsync_SavesMovie));
        var service = new MovieService(context);

        var movie = new Movie
        {
            Title = "Inception",
            Year = 2010,
            Description = "Dreams inside dreams"
        };

        var created = await service.CreateAsync(movie);

        Assert.Equal("Inception", created.Title);
        Assert.NotEqual(0, created.Id);

        var movies = await context.Movies.ToListAsync();
        Assert.Single(movies);
    }

    [Fact]
    public async Task MovieService_GetByIdAsync_ReturnsMovie()
    {
        await using var context = CreateContext(nameof(MovieService_GetByIdAsync_ReturnsMovie));
        context.Movies.Add(new Movie
        {
            Title = "Interstellar",
            Year = 2014,
            Description = "Space stuff"
        });
        await context.SaveChangesAsync();

        var service = new MovieService(context);

        var movie = await service.GetByIdAsync(1);

        Assert.NotNull(movie);
        Assert.Equal("Interstellar", movie!.Title);
    }

    [Fact]
    public async Task RatingService_CreateAsync_ReturnsNull_WhenMovieDoesNotExist()
    {
        await using var context = CreateContext(nameof(RatingService_CreateAsync_ReturnsNull_WhenMovieDoesNotExist));
        var service = new RatingService(context);

        var result = await service.CreateAsync(new Rating
        {
            MovieId = 999,
            Score = 5,
            Comment = "Nice"
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task RatingService_CreateAsync_AddsRating_WhenMovieExists()
    {
        await using var context = CreateContext(nameof(RatingService_CreateAsync_AddsRating_WhenMovieExists));
        context.Movies.Add(new Movie
        {
            Title = "The Matrix",
            Year = 1999,
            Description = "Sci-fi"
        });
        await context.SaveChangesAsync();

        var service = new RatingService(context);

        var result = await service.CreateAsync(new Rating
        {
            MovieId = 1,
            Score = 4,
            Comment = "Good"
        });

        Assert.NotNull(result);
        Assert.Equal(4, result!.Score);

        var ratings = await context.Ratings.ToListAsync();
        Assert.Single(ratings);
    }

    [Fact]
    public async Task AuthService_RegisterAsync_CreatesUser()
    {
        await using var context = CreateContext(nameof(AuthService_RegisterAsync_CreatesUser));
        var service = new AuthService(context, CreateConfig());

        var user = await service.RegisterAsync(new UserDTO
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!"
        });

        Assert.NotNull(user);
        Assert.Equal("testuser", user!.Username);
        Assert.Equal("test@example.com", user.Email);
        Assert.False(string.IsNullOrWhiteSpace(user.PasswordHash));
    }

    [Fact]
    public async Task AuthService_LoginAsync_ReturnsNull_ForWrongPassword()
    {
        await using var context = CreateContext(nameof(AuthService_LoginAsync_ReturnsNull_ForWrongPassword));
        var service = new AuthService(context, CreateConfig());

        await service.RegisterAsync(new UserDTO
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!"
        });

        var result = await service.LoginAsync(new UserDTO
        {
            Email = "test@example.com",
            Password = "WrongPassword"
        });

        Assert.Null(result);
    }
}