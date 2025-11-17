using CombatAnalysis.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Tests;

public class RepositoryTestsBase
{
    protected static CombatParserSQLContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CombatParserSQLContext>()
            .UseInMemoryDatabase(databaseName: dbName + Guid.NewGuid().ToString())
            .Options;

        return new CombatParserSQLContext(options);
    }
}
