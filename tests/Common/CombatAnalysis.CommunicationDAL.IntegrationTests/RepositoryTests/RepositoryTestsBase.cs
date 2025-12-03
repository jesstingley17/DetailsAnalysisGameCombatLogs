using CombatAnalysis.CommunicationDAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.IntegrationTests.RepositoryTests;

public class RepositoryTestsBase
{
    protected static CommunicationContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CommunicationContext>()
            .UseInMemoryDatabase(databaseName: dbName + Guid.NewGuid().ToString())
            .Options;

        return new CommunicationContext(options);
    }
}
