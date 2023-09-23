using System.Security.Claims;
using TestAssignment.Common.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TestAssignment.Common.Interfaces;

/// <summary>
/// Defines the contract for an authentication service responsible for user authentication and JWT token generation.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticate a user based on the provided login view model and generate a JWT token upon successful authentication.
    /// </summary>
    /// <param name="model">The login view model containing user credentials.</param>
    /// <returns>
    /// A JWT token if authentication is successful; otherwise, null.
    /// </returns>
    Task<string?> AuthenticateAsync(LoginViewModel model);
    
    /// <summary>
    /// Gets the user ID from the provided HTTP header dictionary.
    /// </summary>
    /// <param name="headerDictionary">The HTTP header dictionary containing user information.</param>
    /// <returns>The user ID as a string if found; otherwise, null.</returns>
    string GetUserId(IHeaderDictionary headerDictionary);
}