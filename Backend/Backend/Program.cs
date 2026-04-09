using Backend.Data;
using Backend.Interfaces;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Scalar.AspNetCore;
using FeatureHubSDK;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Add feature management service
IFeatureHubConfig fhConfig = new EdgeFeatureHubConfig(
    builder.Configuration["FeatureHub:Host"],
    builder.Configuration["FeatureHub:ApiKey"]
);

await fhConfig.Init();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<RatingService>();
builder.Services.AddSingleton<IFeatureHubConfig>(fhConfig);
builder.Services.AddSingleton<FeatureService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors(options =>
    options
        .WithOrigins("http://localhost:4200", "http://157.173.116.163:8000", "http://157.173.116.163:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
);

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();