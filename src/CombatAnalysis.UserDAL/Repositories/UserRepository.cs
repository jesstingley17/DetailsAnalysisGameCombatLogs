using CombatAnalysis.UserDAL.Data;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;

namespace CombatAnalysis.UserDAL.Repositories;

internal class UserRepository(IConnectionMultiplexer redis, UserContext context) : IUserRepository
{
    private readonly IDatabase _cache = redis.GetDatabase();
    private readonly UserContext _context = context;

    public async Task<AppUser> CreateAsync(AppUser item)
    {
        var entityEntry = await _context.Set<AppUser>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(AppUser item)
    {
        _context.Set<AppUser>().Remove(item);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task<IEnumerable<AppUser>> GetAllAsync()
    {
        var collection = await _context.Set<AppUser>().AsNoTracking().ToListAsync();
        return collection.Count != 0 ? collection : [];
    }

    public async Task<AppUser?> GetByIdAsync(string id)
    {
        var entity = await _context.Set<AppUser>().FindAsync(id);

        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<AppUser?> FindByIdentityUserIdAsync(string identityUserId)
    {
        var entity = await _context.Set<AppUser>().FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<IEnumerable<AppUser>> FindByUsernameStartAtAsync(string prefix)
    {
        var entities = await SearchUsersByPrefixAsync(prefix);
        if (entities.Count != 0)
        {
            return entities;
        }

        entities = await _context.Set<AppUser>()
            .Where(u => u.Username.ToLower().StartsWith(prefix.ToLower()))
            .ToListAsync();

        if (entities.Count != 0)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Detached;
                await AddUserToCacheAsync(entity);
            }

            return entities;
        }

        return [];
    }

    public async Task<AppUser> UpdateAsync(AppUser item)
    {
        var entity = _context.Update(item);
        await _context.SaveChangesAsync();

        return entity.Entity;
    }

    private async Task<List<AppUser>> SearchUsersByPrefixAsync(string prefix)
    {
        string min = $"[{prefix.ToLower()}";
        string max = $"[{prefix.ToLower()}\uffff";

        var result = await _cache.ExecuteAsync("ZRANGE", "usernames", min, max, "BYLEX");

        var raw = (RedisResult[])result!;
        var usernames = raw.Select(r => (string)r!).ToList();

        var users = new List<AppUser>();
        foreach (var username in usernames)
        {
            var json = await _cache.StringGetAsync($"user:{username}");
            if (!json.IsNullOrEmpty)
            {
                users.Add(JsonSerializer.Deserialize<AppUser>(json!)!);
            }
        }

        return users;
    }

    private async Task AddUserToCacheAsync(AppUser user)
    {
        string userKey = $"user:{user.Username.ToLower()}";

        await _cache.StringSetAsync(userKey, JsonSerializer.Serialize(user));

        await _cache.SortedSetAddAsync("usernames", user.Username.ToLower(), 0);
    }
}
