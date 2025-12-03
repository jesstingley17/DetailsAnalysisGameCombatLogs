using CombatAnalysis.IdentityDAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.IntegrationTests.RepositoryTests;

public class RepositoryTestsBase
{
    protected static AppIdentityContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppIdentityContext>()
            .UseInMemoryDatabase(databaseName: dbName + Guid.NewGuid().ToString())
            .Options;

        return new AppIdentityContext(options);
    }
}
