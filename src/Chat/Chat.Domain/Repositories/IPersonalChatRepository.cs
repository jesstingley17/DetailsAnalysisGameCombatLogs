using Chat.Domain.Aggregates;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IPersonalChatRepository : IGenericRepository<PersonalChat, PersonalChatId>
{
    Task UpdateInitiatorUnreadMessageCount(int chatId, int count);

    Task UpdateCompanionUnreadMessageCount(int chatId, int count);
}
