using CombatAnalysis.ChatDAL.Data;
using CombatAnalysis.ChatDAL.DTO;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatDAL.Repositories.SQL.StoredProcedure;

internal class SQLSPGroupChatMessageRepository<TIdType>(ChatSQLContext context) : SQLRepository<GroupChatMessage, TIdType>(context), IGroupChatMessageRepository<TIdType>
    where TIdType : notnull
{
    private readonly ChatSQLContext _context = context;

    public async Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsyn(int chatId, string groupChatUserId, int pageSize)
    {
        var chatIdParam = new SqlParameter("ChatId", chatId);
        var groupChatUserIdParam = new SqlParameter("GroupChatUserId", groupChatUserId);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<GroupChatMessageDto>()
                            .FromSqlRaw($"Get{nameof(GroupChatMessage)}ByChatIdPagination @chatId, @groupChatUserId, @pageSize",
                                            chatIdParam, groupChatUserIdParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetMoreByChatIdAsyn(int chatId, string groupChatUserId, int offset, int pageSize)
    {
        var chatIdParam = new SqlParameter("ChatId", chatId);
        var groupChatUserIdParam = new SqlParameter("GroupChatUserId", groupChatUserId);
        var offsetParam = new SqlParameter("Offset", offset);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<GroupChatMessageDto>()
                            .FromSqlRaw($"Get{nameof(GroupChatMessage)}ByChatIdMore @chatId, @groupChatUserId, @offset, @pageSize",
                                            chatIdParam, groupChatUserIdParam, offsetParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _context.Set<GroupChatMessage>()
                     .CountAsync(cl => cl.ChatId == chatId);

        return count;
    }
}
