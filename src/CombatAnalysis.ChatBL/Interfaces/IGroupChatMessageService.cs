namespace CombatAnalysis.ChatBL.Interfaces;

public interface IGroupChatMessageService<TModel, TIdType> : IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetByChatIdAsync(int chatId, string groupChatUserId, int pageSize);

    Task<IEnumerable<TModel>> GetMoreByChatIdAsync(int chatId, string groupChatUserId, int offset, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}