using CombatAnalysis.ChatDAL.Data;
using CombatAnalysis.ChatDAL.DTO;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatDAL.Repositories.SQL.StoredProcedure;

internal class SQLSPGroupChatMessageRepository<TIdType>(ChatSQLContext context) : SQLRepository<GroupChatMessage, TIdType>(context), IGroupChatMessageRepository<TIdType>
    where TIdType : notnull
{

    public async Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsyn(int chatId, string groupChatUserId, int pageSize)
    {
        var procName = $"Get{nameof(GroupChatMessage)}ByChatIdPagination";
        var data = await _context.Set<GroupChatMessageDto>()
                            .FromSql($"{procName} @chatId={chatId}, @groupChatUserId={groupChatUserId}, @pageSize={pageSize}")
                            .ToListAsync();

        return data;
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetMoreByChatIdAsyn(int chatId, string groupChatUserId, int offset, int pageSize)
    {
        var procName = $"Get{nameof(GroupChatMessage)}ByChatIdMore";
        var data = await _context.Set<GroupChatMessageDto>()
                            .FromSql($"{procName} @chatId={chatId}, @groupChatUserId={groupChatUserId}, @offset={offset}, @pageSize={pageSize}")
                            .ToListAsync();

        return data;
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _context.Set<GroupChatMessage>()
                     .CountAsync(cl => cl.ChatId == chatId);

        return count;
    }
}
