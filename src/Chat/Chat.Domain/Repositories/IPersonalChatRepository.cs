using Chat.Domain.Aggregates;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IPersonalChatRepository : IGenericRepository<PersonalChat, PersonalChatId>
{
    Task UpdateInitiatorUnreadMessageCountAsync(int chatId, int count);

    Task UpdateCompanionUnreadMessageCountAsync(int chatId, int count);
}
