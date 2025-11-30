using Duende.IdentityServer.Models;

namespace IdentityServer.Configuration;

/// <summary>
/// IdentityServer configuration.
/// </summary>
public static class Config
{
    /// <summary>
    /// Gets the identity resources.
    /// </summary>
    /// <returns>The identity resources.</returns>
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "roles",
                DisplayName = "User roles",
                UserClaims = new List<string> { "role" },
            },
        };
    }

    /// <summary>
    /// Gets the clients.
    /// </summary>
    /// <returns>The clients.</returns>
    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "shop-client",
                ClientName = "Shop Client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("shop-secret".Sha256()),
                },
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    "roles",
                    "offline_access",
                },
                AccessTokenLifetime = 900,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 3600,
                UpdateAccessTokenClaimsOnRefresh = true,
            },
        };
    }
}
