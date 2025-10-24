using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using CombatAnalysis.IdentityDAL.Security;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

internal class TokenRepository(AppIdentityContext dbContext) : ITokenRepository
{
    private readonly AppIdentityContext _context = dbContext;

    public async Task<RefreshToken> CreateAsync(string token, int refreshTokenExpiresDays, string clientId, string userId)
    {
        var (hash, salt) = PasswordHashing.HashPasswordWithSalt(token);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid().ToString(),
            TokenHash = hash,
            TokenSalt = salt,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(refreshTokenExpiresDays),
            ClientId = clientId,
            UserId = userId,
        };

        _context.RefreshToken.Add(refreshToken);

        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<int> RotateAsync(string oldRefreshTokenId, string newRefreshTokenId)
    {
        var token = await _context.RefreshToken
            .FirstOrDefaultAsync(t => t.Id == oldRefreshTokenId);
        if (token == null)
        {
            return 0;
        }

        token.RevokedAt = DateTime.UtcNow;
        token.ReplacedByTokenId = newRefreshTokenId;

        _context.Entry(token).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task<int> RevokeAsync(string refreshTokenId)
    {
        var token = await _context.RefreshToken
            .FirstOrDefaultAsync(t => t.Id == refreshTokenId);
        if (token == null)
        {
            return 0;
        }

        token.RevokedAt = DateTime.UtcNow;

        _context.Entry(token).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task<string> ValidateRefreshTokenAsync(string refreshTokenId, string refreshToken, string clientId)
    {
        var token = await _context.RefreshToken
              .FirstOrDefaultAsync(t => t.Id == refreshTokenId && t.ClientId == clientId);
        if (token != null && token.ExpiresAt > DateTime.UtcNow && token.RevokedAt == null)
        {
            var tokenIsValid = PasswordHashing.VerifyPassword(refreshToken, token.TokenHash, token.TokenSalt);
            if (tokenIsValid)
            {
                return token.UserId;
            }
        }

        return string.Empty;
    }

    public async Task<IEnumerable<RefreshToken>?> GetLegitimateTokenByUserIdAsync(string userId)
    {
        var tokens = await _context.RefreshToken
            .Where(t => t.UserId == userId && t.RevokedAt == null && t.ExpiresAt > DateTimeOffset.UtcNow)
            .ToListAsync();

        return tokens;
    }
}
