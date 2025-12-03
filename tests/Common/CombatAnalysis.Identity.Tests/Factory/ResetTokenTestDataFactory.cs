using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.Identity.Tests.Factory;

internal static class ResetTokenTestDataFactory
{
    public static ResetToken Create(
        int? id = null,
        string? email = null,
        string? token = null,
        DateTime? expirationTime = null,
        bool? isUsed = null
        )
    {
        var customer = new ResetToken
        {
            Id = id ?? 1,
            Email = email ?? "email",
            Token = token ?? "token",
            ExpirationTime = expirationTime ?? DateTime.UtcNow,
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
                Token = $"toke-{i}",
                IsUsed = false
            };
        }

        return collection;
    }
}
