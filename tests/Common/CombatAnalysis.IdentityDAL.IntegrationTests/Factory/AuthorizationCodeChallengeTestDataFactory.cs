using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.IntegrationTests.Factory;

internal static class AuthorizationCodeChallengeTestDataFactory
{
    public static AuthorizationCodeChallenge Create(
        string? authorizationCode = null,
        string? codeChallenge = null,
        string? clientId = null,
        string? codeChallengeMethod = null,
        int? expiryTimeDays = null,
        string? redirectUri = null,
        bool? isUsed = null
        )
    {
        var entity = new AuthorizationCodeChallenge
        {
            Id = authorizationCode ?? "uid-1",
            CodeChallenge = codeChallenge ?? "code chalenge",
            ClientId = clientId ?? "client uid",
            CodeChallengeMethod = codeChallengeMethod ?? "code c m",
            CreatedAt = DateTime.UtcNow,
            ExpiryTime = DateTime.UtcNow.AddMinutes(expiryTimeDays ?? 5),
            RedirectUrl = redirectUri ?? "redirect",
            IsUsed = isUsed ?? false
        };

        return entity;
    }

    public static AuthorizationCodeChallenge[] CreateCollection(
        int size = 3 
        )
    {
        var collection = new AuthorizationCodeChallenge[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new AuthorizationCodeChallenge
            {
                Id = $"uid-1-{i}",
                CodeChallenge = "code chalenge",
                ClientId = "client uid",
                CodeChallengeMethod = "code c m",
                CreatedAt = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                RedirectUrl = "redirect",
                IsUsed = false
            };
        }

        return collection;
    }
}
