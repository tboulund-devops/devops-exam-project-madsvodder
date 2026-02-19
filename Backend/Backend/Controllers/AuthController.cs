using Backend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Test user
        public static User user = new();
        
        [HttpPost("register")]
        public ActionResult<User> Register(UserDTO request)
        {
            // Should be handled in a service
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);
            
            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            
            return Ok(user);
        }
    }
}
