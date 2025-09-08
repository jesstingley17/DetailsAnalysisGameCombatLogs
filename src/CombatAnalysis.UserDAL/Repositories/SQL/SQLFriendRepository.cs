using CombatAnalysis.UserDAL.Data;
using CombatAnalysis.UserDAL.DTO;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.UserDAL.Repositories.SQL;

internal class SQLFriendRepository(UserSQLContext context) : IFriendRepository
{
    private readonly UserSQLContext _context = context;

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
                   .Select(x => new FriendDto { Id = x.Id, WhoFriendId = x.WhoFriendId, WhoFriendUsername = x.WhoFriendUsername, ForWhomId = x.ForWhomId, ForWhomUsername = x.ForWhomUsername })
                   .FirstOrDefaultAsync(x => x.Id == entityEntry.Entity.Id);

        return entity;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var model = Activator.CreateInstance<Friend>();
        model.GetType().GetProperty(nameof(Friend.Id))?.SetValue(model, id);

        _context.Set<Friend>().Remove(model);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
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
                          .Select(x => new FriendDto { Id = x.Id, WhoFriendId = x.WhoFriendId, WhoFriendUsername = x.WhoFriendUsername, ForWhomId = x.ForWhomId, ForWhomUsername = x.ForWhomUsername })
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
                          .Select(x => new FriendDto { Id = x.Id, WhoFriendId = x.WhoFriendId, WhoFriendUsername = x.WhoFriendUsername, ForWhomId = x.ForWhomId, ForWhomUsername = x.ForWhomUsername })
                          .FirstOrDefaultAsync(x => x.Id == id);

        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public IEnumerable<FriendDto> GetByParam(string paramName, object value)
    {
        var friends = _context.Set<Friend>()
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
                          .Select(x => new FriendDto { Id = x.Id, WhoFriendId = x.WhoFriendId, WhoFriendUsername = x.WhoFriendUsername, ForWhomId = x.ForWhomId, ForWhomUsername = x.ForWhomUsername })
                          .AsEnumerable();

        var data = friends.Where(x => x.GetType().GetProperty(paramName)?.GetValue(x) == value);

        return data;
    }

    public async Task<int> UpdateAsync(Friend friend)
    {
        _context.Entry(friend).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }
}