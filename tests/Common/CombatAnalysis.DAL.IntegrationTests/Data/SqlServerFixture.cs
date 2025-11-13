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

    public CombatParserSQLContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var connectionString = _container.GetConnectionString();
        var options = new DbContextOptionsBuilder<CombatParserSQLContext>()
            .UseSqlServer(connectionString)
            .Options;

        DbContext = new CombatParserSQLContext(options);

        // Init DB scheme SQL script
        var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../", "../../"));
        await ExecuteSqlScriptAsync(connectionString, $"{solutionRoot}\\databases\\CombatAnalysis.CombatLogs\\InitialCreate.sql");
    }

    public async Task SeedTestDataAsync()
    {
        DbContext.Set<DamageDone>().AddRange(
            new DamageDone { Spell = "Test spell", Value = 50, Time = TimeSpan.FromSeconds(40), Creator = "Player-1", Target = "Enemy-1", DamageType = 0, IsPeriodicDamage = false, IsPet = false, CombatPlayerId = 5 },
            new DamageDone { Spell = "Test spell 2", Value = 150, Time = TimeSpan.FromSeconds(30), Creator = "Player-1", Target = "Enemy-1", DamageType = 0, IsPeriodicDamage = false, IsPet = false, CombatPlayerId = 5 }
        );

        await DbContext.SaveChangesAsync();
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
