using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data;

public class CombatParserContext(DbContextOptions<CombatParserContext> options) : DbContext(options)
{
    public DbSet<Player>? Player { get; }

    public DbSet<Boss>? Boss { get; }

    public DbSet<CombatLog>? CombatLog { get; }

    public DbSet<Combat>? Combat { get; }

    public DbSet<CombatAura>? CombatAura { get; }

    public DbSet<CombatPlayer>? CombatPlayer { get; }

    public DbSet<CombatPlayerPosition>? CombatPlayerPosition { get; }

    public DbSet<PlayerParseInfo>? PlayerParseInfo { get; }

    public DbSet<SpecializationScore>? SpecializationScore { get; }

    public DbSet<DamageDone>? DamageDone { get; }

    public DbSet<DamageDoneGeneral>? DamageDoneGeneral { get; }

    public DbSet<HealDone>? HealDone { get; }

    public DbSet<HealDoneGeneral>? HealDoneGeneral { get; }

    public DbSet<DamageTaken>? DamageTaken { get; }

    public DbSet<DamageTakenGeneral>? DamageTakenGeneral { get; }

    public DbSet<ResourceRecovery>? ResourceRecovery { get; }

    public DbSet<ResourceRecoveryGeneral>? ResourceRecoveryGeneral { get; }

    public DbSet<PlayerDeath>? PlayerDeath { get; }

    public DbSet<PlayerStats>? PlayerStats { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Boss>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Boss>().HasData(MigrationHelper.GenerateBossCollection());
    }
}