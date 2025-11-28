using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

internal class VerifyEmailTokenRepository(AppIdentityContext dbContext) : IVerifyEmailTokenRepository
{
    private readonly AppIdentityContext _context = dbContext;

    public async Task CreateAsync(VerifyEmailToken verifyCode)
    {
        _context.VerifyEmailToken.Add(verifyCode);
        await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(int id, VerifyEmailToken item)
    {
        var existing = await _context.Set<VerifyEmailToken>().FindAsync(id) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<VerifyEmailToken> GetByIdAsync(int id)
    {
        var resetCode = await _context.VerifyEmailToken
            .FirstOrDefaultAsync(c => c.Id == id);

        return resetCode;
    }

    public async Task<VerifyEmailToken> GetByTokenAsync(string token)
    {
        var resetCode = await _context.VerifyEmailToken
            .FirstOrDefaultAsync(c => c.Token == token);

        return resetCode;
    }

    public async Task RemoveExpiredVerifyEmailTokenAsync()
    {
        var tokens = _context.VerifyEmailToken.Where(t => t.ExpirationTime < DateTime.UtcNow);
        _context.VerifyEmailToken.RemoveRange(tokens);

        await _context.SaveChangesAsync();
    }
}
