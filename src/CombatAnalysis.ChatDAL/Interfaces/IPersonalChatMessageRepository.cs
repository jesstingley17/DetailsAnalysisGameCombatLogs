namespace CombatAnalysis.ChatDAL.Interfaces;

public interface IPersonalChatMessageRepository<TModel, TIdType> : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetByChatIdAsyn(int chatId, int pageSize);

    Task<IEnumerable<TModel>> GetMoreByChatIdAsyn(int chatId, int offset, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}