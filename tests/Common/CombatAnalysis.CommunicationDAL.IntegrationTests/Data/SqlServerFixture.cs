using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Post;
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

    public DbContextOptions<CommunicationContext> Options { get; private set; } = null!;

    public CommunicationContext CreateContext()
    {
        return new CommunicationContext(Options);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var connectionString = _container.GetConnectionString();
        Options = new DbContextOptionsBuilder<CommunicationContext>()
            .UseSqlServer(connectionString)
            .Options;

        // Init DB scheme SQL script
        var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../", "../../"));
        await ExecuteSqlScriptAsync(connectionString, $"{solutionRoot}\\databases\\CombatAnalysis.Communication\\InitialCreate.sql");
    }

    public static async Task Drop(CommunicationContext context)
    {
        // Delete all rows
        await context.Database.ExecuteSqlRawAsync("DELETE FROM DamageDone");

        // Reset IDENTITY column to 0 (so next insert starts at 1)
        await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT('DamageDone', RESEED, 0)");
    }

    public static async Task SeedCommunityPostTestDataAsync(CommunicationContext context)
    {
        await context.Set<CommunityPost>().AddRangeAsync(
            new CommunityPost
            {
                CommunityName = "Test again",
                Owner = "Solinx",
                Content = "test content",
                PostType = 1,
                PublicType = 1,
                Restrictions = 0,
                Tags = "tag",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                CommunityId = 1,
                AppUserId = "uid-1-1"
            },
            new CommunityPost
            {
                CommunityName = "Test again 2",
                Owner = "Solinx",
                Content = "test content",
                PostType = 1,
                PublicType = 1,
                Restrictions = 0,
                Tags = "tag",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                CommunityId = 1,
                AppUserId = "uid-1-1"
            },
            new CommunityPost
            {
                CommunityName = "Test again 3",
                Owner = "Solinx",
                Content = "test conten 4",
                PostType = 1,
                PublicType = 1,
                Restrictions = 0,
                Tags = "tag",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                CommunityId = 2,
                AppUserId = "uid-1-1"
            }
        );

        await context.SaveChangesAsync();
    }

    public static async Task SeedUserPostTestDataAsync(CommunicationContext context)
    {
        await context.Set<UserPost>().AddRangeAsync(
            new UserPost
            {
                Owner = "Solinx",
                Content = "test content 1",
                PublicType = 1,
                Tags = "tag",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1",
            },
            new UserPost
            {
                Owner = "Solinx",
                Content = "test content 3",
                PublicType = 1,
                Tags = "tag",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1",
            },
            new UserPost
            {
                Owner = "Solinx",
                Content = "test content 2",
                PublicType = 1,
                Tags = "tag",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-2",
            }
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
