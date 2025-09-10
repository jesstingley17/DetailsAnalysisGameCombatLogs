using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;

namespace CombatAnalysis.Identity.Services;

internal class RefreshTokenService(ITokenRepository tokenRepository) : IRefreshTokenService
{
    private readonly ITokenRepository _tokenRepository = tokenRepository;

    async Task<string> IRefreshTokenService.CreateRefreshTokenAsync(string token, int refreshTokenExpiresDays, string clientId, string userId)
    {
        var refreshToken = await _tokenRepository.CreateAsync(token, refreshTokenExpiresDays, clientId, userId);

        return refreshToken.Id;
    }

    async Task<int> IRefreshTokenService.RotateRefreshTokenAsync(string oldRefreshTokenId, string newRefreshTokenId)
    {
        var rowsAffected = await _tokenRepository.RotateAsync(oldRefreshTokenId, newRefreshTokenId);

        return rowsAffected;
    }

    async Task<int> IRefreshTokenService.RevokeRefreshTokenAsync(string refreshTokenId)
    {
        var rowsAffected = await _tokenRepository.RevokeAsync(refreshTokenId);

        return rowsAffected;
    }

    async Task<string> IRefreshTokenService.ValidateRefreshTokenAsync(string refreshTokenId, string refreshToken, string clientId)
    {
        var userId = await _tokenRepository.ValidateRefreshTokenAsync(refreshTokenId, refreshToken, clientId);
        return userId;
    }

    async Task<IEnumerable<RefreshToken>?> IRefreshTokenService.GetLegitimateTokensByUserIdAsync(string userId)
    {
        var tokens = await _tokenRepository.GetLegitimateTokenByUserIdAsync(userId);

        return tokens;
    }
}
