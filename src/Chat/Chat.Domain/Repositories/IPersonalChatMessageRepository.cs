using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IPersonalChatMessageRepository : IGenericRepository<PersonalChatMessage, PersonalChatMessageId>
{
    Task UpdateAsync(PersonalChatMessage item);

    Task<IEnumerable<PersonalChatMessage>> GetByChatIdAsync(int chatId, int page, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}
