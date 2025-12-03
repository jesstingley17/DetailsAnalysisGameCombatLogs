using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.IntegrationTests.Factory;

internal static class VerifyEmailTokenTestDataFactory
{
    public static VerifyEmailToken Create(
        int? id = null,
        string? email = null,
        string? token = null,
        string? passwordHash = null,
        bool? isUsed = null
        )
    {
        var customer = new VerifyEmailToken
        {
            Id = id ?? 1,
            Email = email ?? "email",
            Token = token ?? "token 23",
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
            IsUsed = isUsed ?? false
        };

        return customer;
    }

    public static VerifyEmailToken[] CreateCollection(
        int size = 3 
        )
    {
        var collection = new VerifyEmailToken[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new VerifyEmailToken
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
