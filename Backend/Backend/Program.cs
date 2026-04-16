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

// Add a feature management service
IFeatureHubConfig fhConfig = new EdgeFeatureHubConfig(
    builder.Configuration["FeatureHub:Host"],
    builder.Configuration["FeatureHub:ApiKey"]
);

try 
{ 
    await fhConfig.Init();
    Console.WriteLine("[FeatureHub] Successfully initialized");
}
catch (Exception ex) 
{ 
    Console.WriteLine($"[FeatureHub] Init failed: {ex.Message}");
}

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
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();