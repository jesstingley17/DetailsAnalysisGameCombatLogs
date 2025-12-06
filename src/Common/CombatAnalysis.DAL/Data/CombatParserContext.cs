using CombatAnalysis.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data;

public class CombatParserContext(DbContextOptions<CombatParserContext> options) : DbContext(options)
{

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
            .Property(e => e.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Boss>().HasData(
            new Boss { Id = 1395, Name = "Каменные стражи" },
            new Boss { Id = 1390, Name = "Фэн Проклятый" },
            new Boss { Id = 1434, Name = "Душелов Гара'джал" },
            new Boss { Id = 1436, Name = "Призрачные короли" },
            new Boss { Id = 1500, Name = "Элегон" },
            new Boss { Id = 1407, Name = "Воля императора" },
            new Boss { Id = 1409, Name = "Вечные защитники" },
            new Boss { Id = 1505, Name = "Цулон" },
            new Boss { Id = 1506, Name = "Лэй Ши" },
            new Boss { Id = 1431, Name = "Ша Страха" },
            new Boss { Id = 1507, Name = "Императорский визирь Зор'лок" },
            new Boss { Id = 1504, Name = "Повелитель клинков Та'як" },
            new Boss { Id = 1463, Name = "Гаралон" },
            new Boss { Id = 1498, Name = "Повелитель ветров Мел'джарак" },
            new Boss { Id = 1499, Name = "Ваятель янтаря Ун'сок" },
            new Boss { Id = 1501, Name = "Великая императрица Шек'зир" }
        );
    }
}