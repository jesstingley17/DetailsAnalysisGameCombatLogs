using Duende.IdentityServer.Models;

namespace CombatAnalysisIdentity.Core;

internal class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("client3scope", "My Scope 3")
        ];

    public static IEnumerable<Client> GetClients()
    {
        return
        [
            new Client
            {
                ClientId = "client3",
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:5003/swagger/oauth2-redirect.html" },

                RequirePkce = true,
                RequireClientSecret = false,

                AllowedScopes = { "client3scope" },
                AllowAccessTokensViaBrowser = true,
            }
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
            new ApiResource("client3scope", "My Scope 3"),
        ];
    }
}
