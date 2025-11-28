using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.IntegrationTests.Factory;

internal static class ResetTokenTestDataFactory
{
    public static ResetToken Create(
        int? id = null,
        string? email = null,
        string? token = null,
        string? passwordHash = null,
        bool? isUsed = null
        )
    {
        var customer = new ResetToken
        {
            Id = id ?? 1,
            Email = email ?? "email",
            Token = token ?? "token 23",
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
            IsUsed = isUsed ?? false
        };

        return customer;
    }

    public static ResetToken[] CreateCollection(
        int size = 3 
        )
    {
        var collection = new ResetToken[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new ResetToken
            {
                Id = 1 + i,
                Email = $"email-{i}",
                Token = $"token 23-{i}",
                ExpirationTime = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };
        }

        return collection;
    }
}
