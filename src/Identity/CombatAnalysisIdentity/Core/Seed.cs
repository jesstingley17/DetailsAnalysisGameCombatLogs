using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;

namespace CombatAnalysisIdentity.Core;

public static class Seed
{
    public static void InitializeIdentity(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        // Seed Clients
        if (!context.Clients.Any())
        {
            foreach (var client in Config.GetClients())
            {
                context.Clients.Add(client.ToEntity());
            }

            context.SaveChanges();
        }

        // Seed IdentityResources
        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.GetIdentityResources())
            {
                context.IdentityResources.Add(resource.ToEntity());
            }

            context.SaveChanges();
        }

        // Seed ApiScopes
        if (!context.ApiScopes.Any())
        {
            foreach (var apiScope in Config.ApiScopes)
            {
                context.ApiScopes.Add(apiScope.ToEntity());
            }

            context.SaveChanges();
        }

        // Seed ApiResources
        if (!context.ApiResources.Any())
        {
            foreach (var apiResource in Config.GetApiResources())
            {
                context.ApiResources.Add(apiResource.ToEntity());
            }

            context.SaveChanges();
        }
    }
}
