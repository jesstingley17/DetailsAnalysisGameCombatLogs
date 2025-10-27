using CombatAnalysis.UserDAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.UserDAL.Tests;

public class RepositoryTestsBase
{
    protected static UserContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<UserContext>()
            .UseInMemoryDatabase(databaseName: dbName + Guid.NewGuid().ToString())
            .Options;

        return new UserContext(options);
    }
}
