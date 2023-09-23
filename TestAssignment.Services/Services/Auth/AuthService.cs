using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Exceptions;
using TestAssignment.Common.Interfaces;
using TestAssignment.Common.ViewModels;

namespace TestAssignment.Services.Services.Auth
{
    /// <summary>
    /// Service responsible for user authentication and JWT token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthProvider _authProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userManager">The UserManager for user management.</param>
        /// <param name="authProvider">The authentication provider for token generation.</param>
        public AuthService(UserManager<User> userManager, IAuthProvider authProvider)
        {
            _userManager = userManager;
            _authProvider = authProvider;
        }

        /// <summary>
        /// Authenticates a user based on the provided login view model and generates a JWT token upon successful authentication.
        /// </summary>
        /// <param name="model">The login view model containing user credentials.</param>
        /// <returns>
        /// A JWT token if authentication is successful; otherwise, null.
        /// </returns>
        public async Task<string?> AuthenticateAsync(LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return _authProvider.GenerateToken(user);
                }

                throw new SecurityTokenException("No user found with the provided username and password");
            }
            catch (Exception ex)
            {
                throw new AuthorizationException("No user found", ex);
            }
        }
        
        /// <summary>
        /// Gets the user ID from the JWT token in the Authorization header.
        /// </summary>
        /// <param name="headerDictionary">The HTTP header dictionary containing the Authorization header.</param>
        /// <returns>The user ID extracted from the JWT token.</returns>
        /// <exception cref="AuthorizationException">Thrown if the user ID claim is not found in the JWT token.</exception>
        public string GetUserId(IHeaderDictionary headerDictionary)
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = _authProvider.ValidateJwtTokenAsync(headerDictionary);
                Claim? userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    throw new SecurityTokenException("No user Id found");

                return userIdClaim.Value;
            }
            catch (Exception ex)
            {
                throw new AuthorizationException("Token is invalid", ex);
            }
        }
    }
}
