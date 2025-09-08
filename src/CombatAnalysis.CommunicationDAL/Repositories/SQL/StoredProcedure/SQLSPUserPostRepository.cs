using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Repositories.SQL.StoredProcedure;

internal class SQLSPUserPostRepository(CommunicationSQLContext context) : SQLRepository<UserPost, int>(context), IUserPostRepository
{
    private readonly CommunicationSQLContext _context = context;

    public async Task<IEnumerable<UserPost>> GetByAppUserIdAsync(string appUserId, int pageSize)
    {
        var procName = $"Get{nameof(UserPost)}ByAppUserIdPagination";
        var data = await Task.Run(() => _context.Set<UserPost>()
                            .FromSql($"{procName} @appUserId={appUserId}, @pageSize={pageSize}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }

    public async Task<IEnumerable<UserPost>> GetMoreByAppUserIdAsync(string appUserId, int offset, int pageSize)
    {
        var procName = $"GetMore{nameof(UserPost)}ByAppUserId";
        var data = await Task.Run(() => _context.Set<UserPost>()
                            .FromSql($"{procName} @appUserId={appUserId}, @offset={offset}, @pageSize={pageSize}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }

    public async Task<IEnumerable<UserPost>> GetNewByAppUserIdAsync(string appUserId, DateTimeOffset checkFrom)
    {
        var procName = $"GetNew{nameof(UserPost)}ByAppUserId";
        var data = await Task.Run(() => _context.Set<UserPost>()
                            .FromSql($"{procName} @appUserId={appUserId}, @checkFrom={checkFrom}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }

    public async Task<IEnumerable<UserPost>> GetByListOfAppUserIdAsync(string appUserIds, int pageSize)
    {
        var procName = $"Get{nameof(UserPost)}ByListOfAppUserIdPagination";
        var data = await Task.Run(() => _context.Set<UserPost>()
                            .FromSql($"{procName} @appUserIds={appUserIds}, @pageSize={pageSize}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }

    public async Task<IEnumerable<UserPost>> GetMoreByListOfAppUserIdAsync(string appUserIds, int offset, int pageSize)
    {
        var procName = $"GetMore{nameof(UserPost)}ByListOfAppUserId";
        var data = await Task.Run(() => _context.Set<UserPost>()
                            .FromSql($"{procName} @appUserIds={appUserIds}, @offset={offset}, @pageSize={pageSize}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }

    public async Task<IEnumerable<UserPost>> GetNewByListOfAppUserIdAsync(string appUserIds, DateTimeOffset checkFrom)
    {
        var procName = $"GetNew{nameof(UserPost)}ByListOfAppUserId";
        var data = await Task.Run(() => _context.Set<UserPost>()
                            .FromSql($"{procName} @appUserIds={appUserIds}, @checkFrom={checkFrom}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }

    public async Task<int> CountByAppUserIdAsync(string appUserId)
    {
        var count = await _context.Set<UserPost>()
                     .CountAsync(cl => cl.AppUserId == appUserId);

        return count;
    }

    public async Task<int> CountByListOfAppUserIdAsync(string[] appUserIds)
    {
        var count = await _context.Set<UserPost>()
                     .CountAsync(cl => appUserIds.Contains(cl.AppUserId));

        return count;
    }
}
