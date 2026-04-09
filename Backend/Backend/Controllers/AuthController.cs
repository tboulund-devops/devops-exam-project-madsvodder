using Backend.Entities;
using Backend.Interfaces;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, IFeatureManager featureManager, FeatureService featureService) : ControllerBase
    {
        
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var user = await authService.RegisterAsync(request);

            if (user == null)
                return BadRequest("Username or email already exists.");

            return Ok(user);
        }

        
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {
            if (! await featureService.IsLoginEnabled())
            {
                return BadRequest("Login is disabled.");
            }
            
            var response = await authService.LoginAsync(request);

            if (response is null) // Adding this comment to test the docker workflow
                return BadRequest("Invalid username or password.");

            return Ok(response);
        }

        [HttpPost("check")]
        public async Task<ActionResult<string>> Check(UserDTO request)
        {
            var message = "Hey this actually works";

            return Ok(message);
        }
    }
}
