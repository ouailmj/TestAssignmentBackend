using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TestAssignment.Common.Entities;

/// <summary>
/// Represents a user entity in the application.
/// </summary>
[Table("AspNetUsers")]
public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the age of the user.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Gets or sets the gender of the user.
    /// </summary>
    public string Gender { get; set; }
}
