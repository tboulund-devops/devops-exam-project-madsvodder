using Backend.Entities;

namespace Backend.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDTO request);

    Task<AuthResponseDto?> LoginAsync(UserDTO request);

    string CreateToken(User user);
}