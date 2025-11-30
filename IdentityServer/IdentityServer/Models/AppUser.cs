using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models;

/// <summary>
/// Application user model extending IdentityUser.
/// </summary>
public class AppUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string? LastName { get; set; }
}
