using CombatAnalysis.ChatDAL.DTO;
using CombatAnalysis.ChatDAL.Entities;

namespace CombatAnalysis.ChatDAL.Interfaces;

public interface IGroupChatMessageRepository<TIdType> : IGenericRepository<GroupChatMessage, TIdType>
    where TIdType : notnull
{
    Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsyn(int chatId, string groupChatUserId, int pageSize);

    Task<IEnumerable<GroupChatMessageDto>> GetMoreByChatIdAsyn(int chatId, string groupChatUserId, int offset, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}
