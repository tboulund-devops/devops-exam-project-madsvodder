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
using Backend.Controllers;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
                ["Jwt:Key"] = "super-secret-test-key-super-secret-test-key-super-secret-test-key-super-secret-test-key",
                ["Jwt:Issuer"] = "test-issuer",
                ["Jwt:Audience"] = "test-audience"
            })
            .Build();
    
    private sealed class FakeAuthService : IAuthService
    {
        private readonly User? _registerResult;
        private readonly AuthResponseDto? _loginResult;

        public FakeAuthService(User? registerResult = null, AuthResponseDto? loginResult = null)
        {
            _registerResult = registerResult;
            _loginResult = loginResult;
        }

        public Task<User?> RegisterAsync(UserDTO request) => Task.FromResult(_registerResult);

        public Task<AuthResponseDto?> LoginAsync(UserDTO request) => Task.FromResult(_loginResult);

        public string CreateToken(User user) => "fake-token";
    }
    
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

    [Fact]
    public async Task MovieService_UpdateAsync_ReturnsFalse_WhenMovieDoesNotExist()
    {
        await using var context = CreateContext(nameof(MovieService_UpdateAsync_ReturnsFalse_WhenMovieDoesNotExist));
        var service = new MovieService(context);

        var result = await service.UpdateAsync(123, new Movie
        {
            Title = "Updated",
            Year = 2020,
            Description = "Updated description"
        });

        Assert.False(result);
    }

    [Fact]
    public async Task MovieService_UpdateAsync_UpdatesMovie_WhenMovieExists()
    {
        await using var context = CreateContext(nameof(MovieService_UpdateAsync_UpdatesMovie_WhenMovieExists));
        context.Movies.Add(new Movie
        {
            Title = "Old title",
            Year = 2000,
            Description = "Old description"
        });
        await context.SaveChangesAsync();

        var service = new MovieService(context);

        var result = await service.UpdateAsync(1, new Movie
        {
            Title = "New title",
            Year = 2024,
            Description = "New description"
        });

        Assert.True(result);

        var movie = await context.Movies.FirstAsync();
        Assert.Equal("New title", movie.Title);
        Assert.Equal(2024, movie.Year);
    }

    [Fact]
    public async Task MovieService_DeleteAsync_ReturnsFalse_WhenMovieDoesNotExist()
    {
        await using var context = CreateContext(nameof(MovieService_DeleteAsync_ReturnsFalse_WhenMovieDoesNotExist));
        var service = new MovieService(context);

        var result = await service.DeleteAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task MovieService_DeleteAsync_RemovesMovie_WhenMovieExists()
    {
        await using var context = CreateContext(nameof(MovieService_DeleteAsync_RemovesMovie_WhenMovieExists));
        context.Movies.Add(new Movie
        {
            Title = "Delete me",
            Year = 2001,
            Description = "Soon gone"
        });
        await context.SaveChangesAsync();

        var service = new MovieService(context);

        var result = await service.DeleteAsync(1);

        Assert.True(result);
        Assert.Empty(await context.Movies.ToListAsync());
    }

    [Fact]
    public async Task RatingService_GetByMovieIdAsync_ReturnsOnlyMatchingRatings()
    {
        await using var context = CreateContext(nameof(RatingService_GetByMovieIdAsync_ReturnsOnlyMatchingRatings));
        context.Movies.Add(new Movie { Title = "Movie 1", Year = 2000, Description = "A" });
        context.Movies.Add(new Movie { Title = "Movie 2", Year = 2001, Description = "B" });
        await context.SaveChangesAsync();

        context.Ratings.AddRange(
            new Rating { MovieId = 1, Score = 4, Comment = "Good" },
            new Rating { MovieId = 1, Score = 5, Comment = "Great" },
            new Rating { MovieId = 2, Score = 1, Comment = "Bad" }
        );
        await context.SaveChangesAsync();

        var service = new RatingService(context);

        var ratings = await service.GetByMovieIdAsync(1);

        Assert.Equal(2, ratings.Count);
        Assert.All(ratings, r => Assert.Equal(1, r.MovieId));
    }

    [Fact]
    public async Task RatingService_GetByIdAsync_ReturnsRating_WhenItExists()
    {
        await using var context = CreateContext(nameof(RatingService_GetByIdAsync_ReturnsRating_WhenItExists));
        context.Movies.Add(new Movie { Title = "Movie", Year = 2000, Description = "Desc" });
        await context.SaveChangesAsync();

        var rating = new Rating { MovieId = 1, Score = 4, Comment = "Nice" };
        context.Ratings.Add(rating);
        await context.SaveChangesAsync();

        var service = new RatingService(context);

        var found = await service.GetByIdAsync(1);

        Assert.NotNull(found);
        Assert.Equal(4, found!.Score);
    }

    [Fact]
    public async Task RatingService_DeleteAsync_ReturnsFalse_WhenRatingDoesNotExist()
    {
        await using var context = CreateContext(nameof(RatingService_DeleteAsync_ReturnsFalse_WhenRatingDoesNotExist));
        var service = new RatingService(context);

        var result = await service.DeleteAsync(1, 1);

        Assert.False(result);
    }

    [Fact]
    public async Task RatingService_DeleteAsync_DeletesRating_WhenItExists()
    {
        await using var context = CreateContext(nameof(RatingService_DeleteAsync_DeletesRating_WhenItExists));
        context.Movies.Add(new Movie { Title = "Movie", Year = 2000, Description = "Desc" });
        await context.SaveChangesAsync();

        context.Ratings.Add(new Rating { MovieId = 1, Score = 5, Comment = "Delete me" });
        await context.SaveChangesAsync();

        var service = new RatingService(context);

        var result = await service.DeleteAsync(1, 1);

        Assert.True(result);
        Assert.Empty(await context.Ratings.ToListAsync());
    }

    [Fact]
    public async Task RatingService_GetAverageScoreAsync_ReturnsAverage_WhenRatingsExist()
    {
        await using var context = CreateContext(nameof(RatingService_GetAverageScoreAsync_ReturnsAverage_WhenRatingsExist));
        context.Movies.Add(new Movie { Title = "Movie", Year = 2000, Description = "Desc" });
        await context.SaveChangesAsync();

        context.Ratings.AddRange(
            new Rating { MovieId = 1, Score = 2, Comment = "A" },
            new Rating { MovieId = 1, Score = 4, Comment = "B" }
        );
        await context.SaveChangesAsync();

        var service = new RatingService(context);

        var avg = await service.GetAverageScoreAsync(1);

        Assert.Equal(3.0, avg);
    }

    [Fact]
    public async Task RatingService_GetAverageScoreAsync_ReturnsNull_WhenNoRatingsExist()
    {
        await using var context = CreateContext(nameof(RatingService_GetAverageScoreAsync_ReturnsNull_WhenNoRatingsExist));
        context.Movies.Add(new Movie { Title = "Movie", Year = 2000, Description = "Desc" });
        await context.SaveChangesAsync();

        var service = new RatingService(context);

        var avg = await service.GetAverageScoreAsync(1);

        Assert.Null(avg);
    }
    
    [Fact]
    public async Task AuthService_RegisterAsync_ReturnsNull_WhenUsernameAlreadyExists()
    {
        await using var context = CreateContext(nameof(AuthService_RegisterAsync_ReturnsNull_WhenUsernameAlreadyExists));
        context.Users.Add(new User
        {
            Username = "testuser",
            Email = "existing@example.com",
            PasswordHash = "hash"
        });
        await context.SaveChangesAsync();

        var service = new AuthService(context, CreateConfig());

        var result = await service.RegisterAsync(new UserDTO
        {
            Username = "testuser",
            Email = "new@example.com",
            Password = "Password123!"
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthService_RegisterAsync_ReturnsNull_WhenEmailAlreadyExists()
    {
        await using var context = CreateContext(nameof(AuthService_RegisterAsync_ReturnsNull_WhenEmailAlreadyExists));
        context.Users.Add(new User
        {
            Username = "existinguser",
            Email = "test@example.com",
            PasswordHash = "hash"
        });
        await context.SaveChangesAsync();

        var service = new AuthService(context, CreateConfig());

        var result = await service.RegisterAsync(new UserDTO
        {
            Username = "newuser",
            Email = "test@example.com",
            Password = "Password123!"
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthService_LoginAsync_ReturnsResponse_WhenPasswordIsCorrect()
    {
        await using var context = CreateContext(nameof(AuthService_LoginAsync_ReturnsResponse_WhenPasswordIsCorrect));
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
            Password = "Password123!"
        });

        Assert.NotNull(result);
        Assert.Equal("testuser", result!.Username);
        Assert.Equal("test@example.com", result.Email);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }
    
    [Fact]
    public async Task MoviesController_GetAll_ReturnsOk()
    {
        await using var context = CreateContext(nameof(MoviesController_GetAll_ReturnsOk));
        context.Movies.Add(new Movie
        {
            Title = "Inception",
            Year = 2010,
            Description = "Dreams"
        });
        await context.SaveChangesAsync();

        var controller = new MoviesController(new MovieService(context));

        var result = await controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }
    
    [Fact]
    public async Task MoviesController_GetSpecific_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        await using var context = CreateContext(nameof(MoviesController_GetSpecific_ReturnsNotFound_WhenMovieDoesNotExist));
        var controller = new MoviesController(new MovieService(context));

        var result = await controller.GetSpecific(123);

        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task MoviesController_Create_ReturnsCreatedAtAction()
    {
        await using var context = CreateContext(nameof(MoviesController_Create_ReturnsCreatedAtAction));
        var controller = new MoviesController(new MovieService(context));

        var result = await controller.Create(new Movie
        {
            Title = "Interstellar",
            Year = 2014,
            Description = "Space"
        });

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(MoviesController.GetSpecific), createdResult.ActionName);
        Assert.NotNull(createdResult.Value);
    }
    
    [Fact]
    public async Task MoviesController_Update_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        await using var context = CreateContext(nameof(MoviesController_Update_ReturnsBadRequest_WhenIdDoesNotMatch));
        var controller = new MoviesController(new MovieService(context));

        var result = await controller.Update(1, new Movie
        {
            Id = 2,
            Title = "Wrong id",
            Year = 2024,
            Description = "Mismatch"
        });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Movie not found. Wrong ID?", badRequest.Value);
    }
    
    [Fact]
    public async Task MoviesController_Delete_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        await using var context = CreateContext(nameof(MoviesController_Delete_ReturnsNotFound_WhenMovieDoesNotExist));
        var controller = new MoviesController(new MovieService(context));

        var result = await controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task MoviesController_Delete_ReturnsNoContent_WhenMovieExists()
    {
        await using var context = CreateContext(nameof(MoviesController_Delete_ReturnsNoContent_WhenMovieExists));
        context.Movies.Add(new Movie
        {
            Title = "Delete me",
            Year = 2001,
            Description = "Soon gone"
        });
        await context.SaveChangesAsync();

        var controller = new MoviesController(new MovieService(context));

        var result = await controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task RatingsController_GetAll_ReturnsOk()
    {
        await using var context = CreateContext(nameof(RatingsController_GetAll_ReturnsOk));
        context.Movies.Add(new Movie { Title = "Movie", Year = 2000, Description = "Desc" });
        await context.SaveChangesAsync();

        context.Ratings.Add(new Rating
        {
            MovieId = 1,
            Score = 4,
            Comment = "Good"
        });
        await context.SaveChangesAsync();

        var controller = new RatingsController(new RatingService(context), null!);

        var result = await controller.GetAll(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }
    
    [Fact]
    public async Task RatingsController_GetAverage_ReturnsOk()
    {
        await using var context = CreateContext(nameof(RatingsController_GetAverage_ReturnsOk));
        context.Movies.Add(new Movie { Title = "Movie", Year = 2000, Description = "Desc" });
        await context.SaveChangesAsync();

        context.Ratings.AddRange(
            new Rating { MovieId = 1, Score = 2, Comment = "A" },
            new Rating { MovieId = 1, Score = 4, Comment = "B" }
        );
        await context.SaveChangesAsync();

        var controller = new RatingsController(new RatingService(context), null!);

        var result = await controller.GetAverage(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }
    
    [Fact]
    public async Task RatingsController_GetAverage_ReturnsNotFound_WhenNoRatingsExist()
    {
        await using var context = CreateContext(nameof(RatingsController_GetAverage_ReturnsNotFound_WhenNoRatingsExist));
        context.Movies.Add(new Movie { Title = "Movie", Year = 2000, Description = "Desc" });
        await context.SaveChangesAsync();

        var controller = new RatingsController(new RatingService(context), null!);

        var result = await controller.GetAverage(1);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No ratings was found for this movie", notFound.Value);
    }
    
    [Fact]
    public async Task RatingsController_Delete_ReturnsNoContent_WhenRatingExists()
    {
        await using var context = CreateContext(nameof(RatingsController_Delete_ReturnsNoContent_WhenRatingExists));
        context.Movies.Add(new Movie { Title = "Movie", Year = 2000, Description = "Desc" });
        await context.SaveChangesAsync();

        context.Ratings.Add(new Rating
        {
            MovieId = 1,
            Score = 5,
            Comment = "Delete me"
        });
        await context.SaveChangesAsync();

        var controller = new RatingsController(new RatingService(context), null!);

        var result = await controller.Delete(1, 1);

        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task RatingsController_Delete_ReturnsNotFound_WhenRatingDoesNotExist()
    {
        await using var context = CreateContext(nameof(RatingsController_Delete_ReturnsNotFound_WhenRatingDoesNotExist));
        context.Movies.Add(new Movie { Title = "Movie", Year = 2000, Description = "Desc" });
        await context.SaveChangesAsync();

        var controller = new RatingsController(new RatingService(context), null!);

        var result = await controller.Delete(1, 999);

        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task AuthController_Register_ReturnsOk()
    {
        var controller = new AuthController(
            new FakeAuthService(new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hash"
            }),
            null!
        );

        var result = await controller.Register(new UserDTO
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!"
        });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }
    
    [Fact]
    public async Task AuthController_Register_ReturnsBadRequest_WhenUserAlreadyExists()
    {
        var controller = new AuthController(new FakeAuthService(null), null!);

        var result = await controller.Register(new UserDTO
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!"
        });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Username or email already exists.", badRequest.Value);
    }
    
    [Fact]
    public async Task AuthController_Check_ReturnsOk()
    {
        var controller = new AuthController(new FakeAuthService(), null!);

        var result = await controller.Check(new UserDTO
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!"
        });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal("Hey this actually works", okResult.Value);
    }
    
    
}