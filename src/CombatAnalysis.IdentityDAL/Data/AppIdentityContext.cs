using CombatAnalysis.IdentityDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Data;

public class AppIdentityContext(DbContextOptions<AppIdentityContext> options) : DbContext(options)
{
    public DbSet<AuthorizationCodeChallenge> AuthorizationCodeChallenge { get; set; }

    public DbSet<RefreshToken> RefreshToken { get; set; }

    public DbSet<IdentityUser> IdentityUser { get; set; }

    public DbSet<ResetToken> ResetToken { get; set; }

    public DbSet<VerifyEmailToken> VerifyEmailToken { get; set; }
}
