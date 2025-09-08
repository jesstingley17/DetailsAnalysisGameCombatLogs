using CombatAnalysis.ChatDAL.Data;
using CombatAnalysis.ChatDAL.Interfaces;
using CombatAnalysis.ChatDAL.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatDAL.Repositories.SQL.StoredProcedure;

internal class SQLSPPersonalChatMessageRepository<TModel, TIdType>(ChatSQLContext context) : SQLRepository<TModel, TIdType>(context), IPersonalChatMessageRepository<TModel, TIdType>
    where TModel : class, IChatEntity
    where TIdType : notnull
{
    private readonly ChatSQLContext _context = context;

    public async Task<IEnumerable<TModel>> GetByChatIdAsyn(int chatId, int pageSize)
    {
        var procName = $"Get{typeof(TModel).Name}ByChatIdPagination";
        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSql($"{procName} @chatId={chatId}, @pageSize={pageSize}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }

    public async Task<IEnumerable<TModel>> GetMoreByChatIdAsyn(int chatId, int offset, int pageSize)
    {
        var procName = $"Get{typeof(TModel).Name}ByChatIdMore";
        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSql($"{procName} @chatId={chatId}, @offset={offset}, @pageSize={pageSize}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(cl => cl.ChatId == chatId);

        return count;
    }
}
