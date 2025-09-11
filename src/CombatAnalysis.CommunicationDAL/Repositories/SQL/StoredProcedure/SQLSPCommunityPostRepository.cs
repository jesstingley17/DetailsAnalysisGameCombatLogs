using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Repositories.SQL.StoredProcedure;

internal class SQLSPCommunityPostRepository(CommunicationSQLContext context) : SQLRepository<CommunityPost, int>(context), ICommunityPostRepository
{
    public async Task<IEnumerable<CommunityPost>> GetByCommunityIdAsync(int communityId, int pageSize)
    {
        var procName = $"Get{nameof(CommunityPost)}ByCommunityIdPagination";
        var data = await _context.Set<CommunityPost>()
                            .FromSql($"{procName} @communityId={communityId}, @pageSize={pageSize}")
                            .ToListAsync();

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetMoreByCommunityIdAsync(int communityId, int offset, int pageSize)
    {
        var procName = $"GetMore{nameof(CommunityPost)}ByCommunityId";
        var data = await _context.Set<CommunityPost>()
                            .FromSql($"GetMore{nameof(CommunityPost)}ByCommunityId @communityId={communityId}, @offset={offset}, @pageSize={pageSize}")
                            .ToListAsync();

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetNewByCommunityIdAsync(int communityId, DateTimeOffset checkFrom)
    {
        var procName = $"GetNew{nameof(CommunityPost)}ByCommunityId";
        var data = await _context.Set<CommunityPost>()
                            .FromSql($"{procName} @communityId={communityId}, @checkFrom={checkFrom}")
                            .ToListAsync();

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetByListOfCommunityIdAsync(string communityIds, int pageSize)
    {
        var procName = $"Get{nameof(CommunityPost)}ByListOfCommunityIdPagination";
        var data = await _context.Set<CommunityPost>()
                            .FromSql($"{procName} @communityIds={communityIds}, @pageSize={pageSize}")
                            .ToListAsync();

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetMoreByListOfCommunityIdAsync(string communityIds, int offset, int pageSize)
    {
        var procName = $"GetMore{nameof(CommunityPost)}ByListOfCommunityId";
        var data = await _context.Set<CommunityPost>()
                            .FromSql($"{procName} @communityIds={communityIds}, @offset={offset}, @pageSize={pageSize}")
                            .ToListAsync();

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetNewByListOfCommunityIdAsync(string communityIds, DateTimeOffset checkFrom)
    {
        var procName = $"GetNew{nameof(CommunityPost)}ByListOfCommunityId";
        var data = await _context.Set<CommunityPost>()
                            .FromSql($"{procName} @communityIds={communityIds}, @checkFrom={checkFrom}")
                            .ToListAsync();

        return data;
    }

    public async Task<int> CountByCommunityIdAsync(int communityId)
    {
        var count = await _context.Set<CommunityPost>()
                     .CountAsync(cl => cl.CommunityId == communityId);

        return count;
    }

    public async Task<int> CountByListOfCommunityIdAsync(int[] communityIds)
    {
        var count = await _context.Set<CommunityPost>()
                     .CountAsync(cl => communityIds.Contains(cl.CommunityId));

        return count;
    }
}
