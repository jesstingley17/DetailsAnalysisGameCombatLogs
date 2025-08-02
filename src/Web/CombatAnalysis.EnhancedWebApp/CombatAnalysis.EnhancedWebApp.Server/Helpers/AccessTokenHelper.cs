using System.IdentityModel.Tokens.Jwt;

namespace CombatAnalysis.EnhancedWebApp.Server.Helpers;

internal static class AccessTokenHelper
{
    public static string GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "sub");

        return userIdClaim?.Value ?? string.Empty;
    }

    public static DateTime GetValidToFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        return jsonToken?.ValidTo ?? DateTime.UtcNow;
    }
}
