using CombatAnalysis.UserDAL.Data;
using CombatAnalysis.UserDAL.DTO;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.UserDAL.Repositories;

internal class FriendRepository(UserContext context) : IFriendRepository
{
    private readonly UserContext _context = context;

    public async Task<FriendDto?> CreateAsync(Friend friend)
    {
        var entityEntry = await _context.Set<Friend>().AddAsync(friend);
        await _context.SaveChangesAsync();

        var entity = await (
            from f in _context.Set<Friend>()
            join who in _context.Set<AppUser>() on f.WhoFriendId equals who.Id
            join whom in _context.Set<AppUser>() on f.ForWhomId equals whom.Id
            where f.Id == entityEntry.Entity.Id
            select new FriendDto(
                f.Id,
                f.WhoFriendId,
                who.Username,
                f.ForWhomId,
                whom.Username
            )
        ).FirstOrDefaultAsync();

        return entity;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var entity = await _context.Set<Friend>().FindAsync(id);

        if (entity == null)
        {
            return 0;
        }

        _context.Set<Friend>().Remove(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<FriendDto>> GetAllAsync()
    {
        var query = from f in _context.Set<Friend>()
                    join who in _context.Set<AppUser>() on f.WhoFriendId equals who.Id
                    join whom in _context.Set<AppUser>() on f.ForWhomId equals whom.Id
                    select new FriendDto(
                        f.Id,
                        f.WhoFriendId,
                        who.Username,
                        f.ForWhomId,
                        whom.Username
                    );

        var result = await query.ToListAsync();

        return result;
    }

    public async Task<FriendDto?> GetByIdAsync(int id)
    {
        var query = from f in _context.Set<Friend>()
                    join who in _context.Set<AppUser>() on f.WhoFriendId equals who.Id
                    join whom in _context.Set<AppUser>() on f.ForWhomId equals whom.Id
                    where f.Id == id
                    select new FriendDto(
                        f.Id,
                        f.WhoFriendId,
                        who.Username,
                        f.ForWhomId,
                        whom.Username
                    );

        var result = await query.FirstOrDefaultAsync();

        return result;
    }

    public async Task<IEnumerable<FriendDto>> GetByParamAsync(string paramName, object value)
    {
        var query = from f in _context.Set<Friend>()
                    join who in _context.Set<AppUser>() on f.WhoFriendId equals who.Id
                    join whom in _context.Set<AppUser>() on f.ForWhomId equals whom.Id
                    where f.ForWhomId == (string)value || f.WhoFriendId == (string)value
                    select new FriendDto(
                        f.Id,
                        f.WhoFriendId,
                        who.Username,
                        f.ForWhomId,
                        whom.Username
                    );

        var result = await query.ToListAsync();

        return result;
    }
}