using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

/// <summary>
/// Profile service for including user roles in tokens.
/// </summary>
public class ProfileService : IProfileService
{
    private readonly UserManager<AppUser> userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileService"/> class.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    public ProfileService(UserManager<AppUser> userManager)
    {
        this.userManager = userManager;
    }

    /// <summary>
    /// Gets profile data for the user.
    /// </summary>
    /// <param name="context">Profile data request context.</param>
    /// <returns>Task.</returns>
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await this.userManager.GetUserAsync(context.Subject);
        if (user != null)
        {
            var roles = await this.userManager.GetRolesAsync(user);
            var claims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            context.IssuedClaims.AddRange(claims);
        }
    }

    /// <summary>
    /// Determines if the user is active.
    /// </summary>
    /// <param name="context">Is active context.</param>
    /// <returns>Task.</returns>
    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}
