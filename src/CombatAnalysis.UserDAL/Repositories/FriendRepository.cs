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

        var entity = await _context.Set<Friend>()
                  .Join(_context.Set<AppUser>(),
                     f => f.WhoFriendId,
                     u => u.Id,
                     (f, u) => new
                     {
                         f.Id,
                         f.WhoFriendId,
                         f.ForWhomId,
                         WhoFriendUsername = u.Username,
                     })
                   .Join(_context.Set<AppUser>(),
                     temp => temp.ForWhomId,
                     u => u.Id,
                     (temp, u) => new
                     {
                         temp.Id,
                         temp.WhoFriendId,
                         temp.WhoFriendUsername,
                         temp.ForWhomId,
                         ForWhomUsername = u.Username
                     })
                   .Select(x => new FriendDto(x.Id, x.WhoFriendId, x.WhoFriendUsername, x.ForWhomId, x.ForWhomUsername))
                   .FirstOrDefaultAsync(x => x.Id == entityEntry.Entity.Id);

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
        var friends = await _context.Set<Friend>()
                         .Join(_context.Set<AppUser>(),
                            f => f.WhoFriendId,
                            u => u.Id,
                            (f, u) => new
                            {
                                f.Id,
                                f.WhoFriendId,
                                f.ForWhomId,
                                WhoFriendUsername = u.Username,
                            })
                          .Join(_context.Set<AppUser>(),
                            temp => temp.ForWhomId,
                            u => u.Id,
                            (temp, u) => new
                            {
                                temp.Id,
                                temp.WhoFriendId,
                                temp.WhoFriendUsername,
                                temp.ForWhomId,
                                ForWhomUsername = u.Username
                            })
                          .Select(x => new FriendDto(x.Id, x.WhoFriendId, x.WhoFriendUsername, x.ForWhomId, x.ForWhomUsername))
                          .ToListAsync();

        return friends.Count != 0 ? friends : [];
    }

    public async Task<FriendDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Set<Friend>()
                         .Join(_context.Set<AppUser>(),
                            f => f.WhoFriendId,
                            u => u.Id,
                            (f, u) => new
                            {
                                f.Id,
                                f.WhoFriendId,
                                f.ForWhomId,
                                WhoFriendUsername = u.Username,
                            })
                          .Join(_context.Set<AppUser>(),
                            temp => temp.ForWhomId,
                            u => u.Id,
                            (temp, u) => new
                            {
                                temp.Id,
                                temp.WhoFriendId,
                                temp.WhoFriendUsername,
                                temp.ForWhomId,
                                ForWhomUsername = u.Username
                            })
                          .Select(x => new FriendDto(x.Id, x.WhoFriendId, x.WhoFriendUsername, x.ForWhomId, x.ForWhomUsername))
                          .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<IEnumerable<FriendDto>> GetByParamAsync(string paramName, object value)
    {
        var friends = await _context.Set<Friend>()
                         .Join(_context.Set<AppUser>(),
                            f => f.WhoFriendId,
                            u => u.Id,
                            (f, u) => new
                            {
                                f.Id,
                                f.WhoFriendId,
                                f.ForWhomId,
                                WhoFriendUsername = u.Username,
                            })
                          .Join(_context.Set<AppUser>(),
                            temp => temp.ForWhomId,
                            u => u.Id,
                            (temp, u) => new
                            {
                                temp.Id,
                                temp.WhoFriendId,
                                temp.WhoFriendUsername,
                                temp.ForWhomId,
                                ForWhomUsername = u.Username
                            })
                          .Select(x => new FriendDto(x.Id, x.WhoFriendId, x.WhoFriendUsername, x.ForWhomId, x.ForWhomUsername))
                          .ToListAsync();

        var data = friends
                    .Where(x => x.GetType().GetProperty(paramName)?.GetValue(x) == value)
                    .ToList();

        return data;
    }
}