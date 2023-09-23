using Microsoft.AspNetCore.Mvc;
using TestAssignment.Common.Interfaces;
using TestAssignment.Common.ViewModels;

namespace TestAssignment.Controllers
{
    /// <summary>
    /// Controller responsible for handling user authentication and JWT token generation.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service for user authentication and token generation.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticate a user and generate a JWT token.
        /// </summary>
        /// <param name="model">The login view model containing user credentials.</param>
        /// <returns>
        /// Ok with a JWT token if authentication is successful;
        /// Unauthorized if authentication fails.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var token = await _authService.AuthenticateAsync(model);
            if (token != null)
            {
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}