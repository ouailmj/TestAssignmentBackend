namespace TestAssignment.Common.Config;

/// <summary>
/// Represents settings related to JSON Web Tokens (JWT) used for authentication and authorization.
/// </summary>
public class JWTSettings
{
    /// <summary>
    /// Gets or sets the secret key used for JWT token generation and validation.
    /// </summary>
    public String Key { get; set; }
    
    /// <summary>
    /// Gets or sets the issuer of JWT tokens. It typically identifies the entity that issued the token.
    /// </summary>
    public String Issuer { get; set; }
    
    /// <summary>
    /// Gets or sets the audience of JWT tokens. It specifies the intended recipient of the token.
    /// </summary>
    public String Audience { get; set; }
    
    /// <summary>
    /// Gets or sets the number of days for which a JWT token remains valid.
    /// </summary>
    public int ExpireDays { get; set; }

}