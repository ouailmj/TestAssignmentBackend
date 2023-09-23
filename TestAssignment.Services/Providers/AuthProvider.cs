using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using TestAssignment.Common.Config;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Interfaces;
using SecurityTokenException = TestAssignment.Common.Exceptions.SecurityTokenException;

namespace TestAssignment.Services.Providers;

/// <summary>
/// Provides authentication-related functionality, including JWT token generation and validation.
/// </summary>
public class AuthProvider : IAuthProvider
{
    private readonly JWTSettings _jwtSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthProvider"/> class.
    /// </summary>
    /// <param name="jwtSettings">The JWT settings.</param>
    public AuthProvider(JWTSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    /// <inheritdoc />
    public string GenerateToken(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_jwtSettings.ExpireDays);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception e)
        {
            throw new SecurityTokenException("Error when generating token", e);
        }
    }

    /// <inheritdoc />
    public ClaimsPrincipal ValidateJwtTokenAsync(IHeaderDictionary headerDictionary)
    {
        try
        {
            if (!headerDictionary.ContainsKey("Authorization"))
            {
                throw new AuthenticationException("Authorization header missing.");
            }

            // Get the JWT token from the Authorization header
            string token = headerDictionary["Authorization"].ToString().Replace("Bearer ", "");

            // Verify and decode the JWT token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
            }, out securityToken);

            return claimsPrincipal;
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException("Error When validating the token", ex);
        }
    }
}