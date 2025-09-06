using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Data;

public class IdentityContext(DbContextOptions<IdentityContext> options) : DbContext(options)
{
    public DbSet<AuthorizationCodeChallenge> AuthorizationCodeChallenge { get; set; }

    public DbSet<RefreshToken> RefreshToken { get; set; }

    public DbSet<IdentityUser> IdentityUser { get; set; }

    public DbSet<ResetToken> ResetToken { get; set; }

    public DbSet<VerifyEmailToken> VerifyEmailToken { get; set; }

    public DbSet<Client> Client { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>().HasData(
            new Client
            {
                Id = Guid.NewGuid().ToString(),
                RedirectUrl = "localhost:45571/callback",
                AllowedScopes = "api.read,api.write",
                AllowedAudiences = "user-api,chat-api,communication-api,hubs,notification-api",
                ClientName = "desktop",
                ClientType = (int)ClientType.Public,
                IsActive = true,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now
            },
            new Client
            {
                Id = Guid.NewGuid().ToString(),
                RedirectUrl = "localhost:5173/callback",
                AllowedScopes = "api.read,api.write",
                AllowedAudiences = "user-api,chat-api,communication-api,hubs,notification-api",
                ClientName = "web",
                ClientType = (int)ClientType.Public,
                IsActive = true,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now
            }
        );
    }
}
