using Duende.IdentityServer.Models;

namespace CombatAnalysisIdentity.Core;

internal class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("api.read", "Allow read to API"),
            new ApiScope("api.write", "Allow write to API"),
        ];

    public static IEnumerable<Client> GetClients()
    {
        return
        [
            new Client
            {
                ClientId = "desktop-app",
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:5003/swagger/oauth2-redirect.html" },

                RequirePkce = true,
                RequireClientSecret = false,

                AllowedScopes = { "api.read", "api.write" },
                AllowAccessTokensViaBrowser = true,
            },
            new Client
            {
                ClientId = "web-app",
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:5003/swagger/oauth2-redirect.html" },

                RequirePkce = true,
                RequireClientSecret = false,

                AllowedScopes = { "api.read", "api.write" },
                AllowAccessTokensViaBrowser = true,
            },
        ];
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        ];
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return
        [
            new ApiResource("userapi", "Protected User API")
            {
                Scopes = { "api.read", "api.write" },
                UserClaims = { "aud" }
            },
            new ApiResource("chatapi", "Protected Chat API")
            {
                Scopes = { "api.read", "api.write" },
                UserClaims = { "aud" }
            },
        ];
    }
}
