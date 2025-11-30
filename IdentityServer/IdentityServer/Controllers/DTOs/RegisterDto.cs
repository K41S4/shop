using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Controllers.DTOs;

/// <summary>
/// User registration DTO.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [Required]
    [MinLength(3)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role.
    /// </summary>
    [Required]
    public string Role { get; set; } = string.Empty;
}
