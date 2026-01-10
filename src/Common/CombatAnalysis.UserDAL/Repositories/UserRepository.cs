using CombatAnalysis.UserDAL.Data;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.UserDAL.Repositories;

//internal class UserRepository(IConnectionMultiplexer redis, UserContext context) : IUserRepository
internal class UserRepository(UserContext context) : IUserRepository
{
    //private readonly IDatabase _cache = redis.GetDatabase();
    private readonly UserContext _context = context;

    public async Task<AppUser> CreateAsync(AppUser item)
    {
        var entityEntry = await _context.Set<AppUser>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> UpdateAsync(string id, AppUser item)
    {
        var existing = await _context.Set<AppUser>().FindAsync(id) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await _context.Set<AppUser>().FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Set<AppUser>().Remove(entity);
        await _context.SaveChangesAsync();

        return true;
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
        //var entities = await SearchUsersByPrefixAsync(prefix);
        //if (entities.Count != 0)
        //{
        //    return entities;
        //}

        var entities = await _context.Set<AppUser>()
            .Where(u => u.Username.ToLower().StartsWith(prefix.ToLower()))
            .ToListAsync();

        if (entities.Count != 0)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Detached;
                //await AddUserToCacheAsync(entity);
            }

            return entities;
        }

        return [];
    }

    //private async Task<List<AppUser>> SearchUsersByPrefixAsync(string prefix)
    //{
    //    string min = $"[{prefix.ToLower()}";
    //    string max = $"[{prefix.ToLower()}\uffff";

    //    var result = await _cache.ExecuteAsync("ZRANGE", "usernames", min, max, "BYLEX");

    //    var raw = result.IsNull ? Array.Empty<RedisResult>() : (RedisResult[])result!;
    //    var usernames = raw.Select(r => (string)r!).ToList();

    //    var users = new List<AppUser>();
    //    foreach (var username in usernames)
    //    {
    //        var json = await _cache.StringGetAsync($"user:{username}");
    //        if (!json.IsNullOrEmpty)
    //        {
    //            users.Add(JsonSerializer.Deserialize<AppUser>(json!)!);
    //        }
    //    }

    //    return users;
    //}

    //private async Task AddUserToCacheAsync(AppUser user)
    //{
    //    string userKey = $"user:{user.Username.ToLower()}";

    //    await _cache.StringSetAsync(userKey, JsonSerializer.Serialize(user));

    //    await _cache.SortedSetAddAsync("usernames", user.Username.ToLower(), 0);
    //}
}
