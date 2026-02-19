using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.Entities;
using Backend.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class AuthService(AppDbContext context, IConfiguration config) : IAuthService
{
    public async Task<User?> RegisterAsync(UserDTO request)
    {
        if (await context.Users.AnyAsync(u => u.Username.ToLower() == request.Username.ToLower()))
            return null;

        if (await context.Users.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower()))
            return null;

        var user = new User();

        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, request.Password);

        user.Username = request.Username;
        user.Email = request.Email;
        user.PasswordHash = hashedPassword;

        context.Users.Add(user); // Only tracked (not saved) until SaveChangesAsync is called
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<AuthResponseDto?> LoginAsync(UserDTO request)
    {

        var user = await context.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (user is null)
            return null;

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) ==
            PasswordVerificationResult.Failed)
            return null;

        var token = CreateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email
        };
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}