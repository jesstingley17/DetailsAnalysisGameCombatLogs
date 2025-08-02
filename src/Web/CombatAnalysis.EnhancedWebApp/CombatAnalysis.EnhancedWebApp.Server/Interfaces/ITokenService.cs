using CombatAnalysis.EnhancedWebApp.Server.Models.Identity;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces;

public interface ITokenService
{
    Task<AccessTokenModel> RefreshAccessTokenAsync(string refreshToken);

    bool IsAccessTokenCloseToExpiry(string accessToken);
}
