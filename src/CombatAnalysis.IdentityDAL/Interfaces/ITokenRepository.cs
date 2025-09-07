using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface ITokenRepository
{
    Task<RefreshToken> CreateAsync(string token, int refreshTokenExpiresDays, string clientId, string userId);

    Task<int> RotateAsync(string oldRefreshTokenId, string newRefreshTokenId);

    Task<int> RevokeAsync(string refreshTokenId);

    Task<string> ValidateRefreshTokenAsync(string refreshTokenId, string refreshToken, string clientId);
}
