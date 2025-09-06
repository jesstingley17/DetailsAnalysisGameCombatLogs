using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

internal class TokenRepository(IdentityContext dbContext) : ITokenRepository
{
    private readonly IdentityContext _context = dbContext;

    public async Task SaveAsync(string token, int refreshTokenExpiresDays, string clientId, string userId)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid().ToString(),
            Token = token,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(refreshTokenExpiresDays),
            ClientId = clientId,
            UserId = userId,
        };

        _context.RefreshToken.Add(refreshToken);

        await _context.SaveChangesAsync();
    }

    public async Task<string> ValidateRefreshTokenAsync(string refreshToken, string clientId)
    {
        var tokenEntry = await _context.RefreshToken
            .FirstOrDefaultAsync(t => t.Token == refreshToken && t.ClientId == clientId);
        if (tokenEntry != null && tokenEntry.ExpiresAt > DateTime.UtcNow)
        {
            return tokenEntry.UserId;
        }

        return string.Empty;
    }

    public async Task RemoveExpiredTokensAsync()
    {
        var expiredTokens = _context.RefreshToken.Where(t => t.ExpiresAt < DateTime.UtcNow);
        _context.RefreshToken.RemoveRange(expiredTokens);

        await _context.SaveChangesAsync();
    }
}
