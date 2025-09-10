using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.Identity.Interfaces;

public interface IRefreshTokenService
{
    Task<string> CreateRefreshTokenAsync(string token, int refreshTokenExpiresDays, string clientId, string userId);

    Task<int> RotateRefreshTokenAsync(string oldRefreshTokenId, string newRefreshTokenId);

    Task<int> RevokeRefreshTokenAsync(string refreshTokenId);

    Task<string> ValidateRefreshTokenAsync(string refreshTokenId, string refreshToken, string clientId);

    Task<IEnumerable<RefreshToken>?> GetLegitimateTokensByUserIdAsync(string userId);
}
