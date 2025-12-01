using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

/// <summary>
/// Profile service for including user roles and permissions in tokens.
/// </summary>
public class ProfileService : IProfileService
{
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileService"/> class.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    /// <param name="roleManager">Role manager.</param>
    public ProfileService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
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

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            context.IssuedClaims.AddRange(roleClaims);

            foreach (var roleName in roles)
            {
                var role = await this.roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    continue;
                }

                var claims = await this.roleManager.GetClaimsAsync(role);
                var rolePermissionClaims = claims
                    .Where(c => c.Type == "permission")
                    .Select(c => new Claim("permission", c.Value));
                context.IssuedClaims.AddRange(rolePermissionClaims);
            }
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
