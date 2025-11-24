using Chat.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.IntegrationTests.RepositoryTests;

public class RepositoryTestsBase
{
    protected static ChatContext CreateInMemoryContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ChatContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ChatContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
