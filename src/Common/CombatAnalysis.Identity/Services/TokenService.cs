using CombatAnalysis.Identity.Interfaces;
using System.Security.Cryptography;

namespace CombatAnalysis.Identity.Services;

internal class TokenService : IToken
{
    public string GenerateToken()
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        var randomBytes = new byte[32]; // 256 bits
        randomNumberGenerator.GetBytes(randomBytes);

        var code = Convert.ToBase64String(randomBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");

        return code;
    }
}
