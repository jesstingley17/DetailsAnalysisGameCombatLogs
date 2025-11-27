using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace CombatAnalysis.DAL.IntegrationTests.Data;

public class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container;

    public SqlServerFixture()
    {
        _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Oleg123*")
            .Build();
    }

    public DbContextOptions<CombatParserContext> Options { get; private set; } = null!;

    public CombatParserContext CreateContext()
    {
        return new CombatParserContext(Options);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var connectionString = _container.GetConnectionString();
        Options = new DbContextOptionsBuilder<CombatParserContext>()
            .UseSqlServer(connectionString)
            .Options;

        // Init DB scheme SQL script
        var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../", "../../"));
        await ExecuteSqlScriptAsync(connectionString, $"{solutionRoot}\\databases\\CombatAnalysis.CombatLogs\\InitialCreate.sql");
    }

    public static async Task Drop(CombatParserContext context)
    {
        // Delete all rows
        await context.Database.ExecuteSqlRawAsync("DELETE FROM DamageDone");

        // Reset IDENTITY column to 0 (so next insert starts at 1)
        await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT('DamageDone', RESEED, 0)");
    }

    public static async Task SeedDamageDoneTestDataAsync(CombatParserContext context)
    {
        await context.Set<DamageDone>().AddRangeAsync(
            new DamageDone { Spell = "Test spell", Value = 50, Time = TimeSpan.FromSeconds(40), Creator = "Player-1", Target = "Enemy-1", DamageType = 0, IsPeriodicDamage = false, IsPet = false, CombatPlayerId = 5 },
            new DamageDone { Spell = "Test spell 2", Value = 150, Time = TimeSpan.FromSeconds(30), Creator = "Player-1", Target = "Enemy-1", DamageType = 0, IsPeriodicDamage = false, IsPet = false, CombatPlayerId = 5 }
        );

        await context.SaveChangesAsync();
    }

    public static async Task SeedSpecializationScoreTestDataAsync(CombatParserContext context)
    {
        await context.Set<SpecializationScore>().AddRangeAsync(
            new SpecializationScore { SpecId = 1, BossId = 1, Difficult = 1, Damage = 1233321, Heal = 1231, Updated = null },
            new SpecializationScore { SpecId = 1, BossId = 2, Difficult = 1, Damage = 432112, Heal = 2234142, Updated = null }
        );

        await context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }

    private static async Task ExecuteSqlScriptAsync(string connectionString, string scriptPath)
    {
        var sql = await File.ReadAllTextAsync(scriptPath);

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var commands = sql.Split(
            ["GO", "go", "Go"],
            StringSplitOptions.RemoveEmptyEntries);

        foreach (var commandText in commands)
        {
            if (string.IsNullOrWhiteSpace(commandText)) continue;

            await using var command = new SqlCommand(commandText, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
