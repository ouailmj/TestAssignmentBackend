using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TestAssignment.Common.Entities;

namespace TestAssignment.Common.Interfaces
{
    /// <summary>
    /// Provides an interface for authentication and token-related operations.
    /// </summary>
    public interface IAuthProvider
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token will be generated.</param>
        /// <returns>A JWT token as a string.</returns>
        string GenerateToken(User user);

        /// <summary>
        /// Validates a JWT token from the provided HTTP header dictionary.
        /// </summary>
        /// <param name="headerDictionary">The HTTP header dictionary containing the token.</param>
        /// <returns>A ClaimsPrincipal representing the validated user if successful; otherwise, null.</returns>
        ClaimsPrincipal ValidateJwtTokenAsync(IHeaderDictionary headerDictionary);
    }
}
