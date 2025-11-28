using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

internal class IdentityUserRepository(AppIdentityContext dbContext) : IIdentityUserRepository
{
    private readonly AppIdentityContext _context = dbContext;

    public async Task SaveAsync(IdentityUser identityUser)
    {
        _context.IdentityUser.Add(identityUser);
        await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(string id, IdentityUser item)
    {
        var existing = await _context.Set<IdentityUser>().FindAsync(id) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<IdentityUser?> GetByIdAsync(string id)
    {
        var identityUser = await _context.IdentityUser
            .FirstOrDefaultAsync(c => c.Id == id);

        return identityUser;
    }

    public async Task<bool> CheckByEmailAsync(string email)
    {
        var identityUser = await _context.IdentityUser
            .FirstOrDefaultAsync(c => c.Email == email);
        var userPresent = identityUser != null;

        return userPresent;
    }

    public async Task<IdentityUser?> GetAsync(string email)
    {
        var entity = await _context.Set<IdentityUser>()
            .FirstOrDefaultAsync(x => x.Email == email);

        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }
}
