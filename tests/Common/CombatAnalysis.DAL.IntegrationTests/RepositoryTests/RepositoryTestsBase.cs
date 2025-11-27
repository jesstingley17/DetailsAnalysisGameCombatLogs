using CombatAnalysis.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.IntegrationTests.RepositoryTests;

public class RepositoryTestsBase
{
    protected static CombatParserContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CombatParserContext>()
            .UseInMemoryDatabase(databaseName: dbName + Guid.NewGuid().ToString())
            .Options;

        return new CombatParserContext(options);
    }
}
