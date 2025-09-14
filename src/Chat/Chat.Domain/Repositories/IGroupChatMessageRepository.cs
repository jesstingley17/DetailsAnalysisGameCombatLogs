using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IGroupChatMessageRepository : IGenericRepository<GroupChatMessage, GroupChatMessageId>
{
    Task UpdateAsync(GroupChatMessage item);

    Task<IEnumerable<GroupChatMessage>> GetByChatIdAsync(int chatId, int page, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}
