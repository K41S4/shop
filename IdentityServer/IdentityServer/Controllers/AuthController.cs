using IdentityServer.Controllers.DTOs;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

/// <summary>
/// Authentication controller.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    public AuthController(UserManager<AppUser> userManager)
    {
        this.userManager = userManager;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="dto">User data.</param>
    /// <returns>The response.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new AppUser
        {
            UserName = dto.Username,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
        };

        var result = await this.userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return this.BadRequest(result.Errors);
        }

        await this.userManager.AddToRoleAsync(user, dto.Role);
        return this.Ok("User registered");
    }
}
