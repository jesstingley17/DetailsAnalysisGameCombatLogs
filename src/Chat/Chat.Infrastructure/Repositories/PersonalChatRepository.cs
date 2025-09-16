using Chat.Domain.Aggregates;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;

namespace Chat.Infrastructure.Repositories;

internal class PersonalChatRepository(ChatContext context) : GenericRepository<PersonalChat, PersonalChatId>(context), IPersonalChatRepository
{
    public async Task UpdateInitiatorUnreadMessageCountAsync(int chatId, int count)
    {
        var personalChat = await GetByIdAsync(chatId)
                    ?? throw new EntityNotFoundException(typeof(PersonalChat), chatId);

        personalChat.UpdateInitiatorUnreadMessageCount(count);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateCompanionUnreadMessageCountAsync(int chatId, int count)
    {
        var personalChat = await GetByIdAsync(chatId)
                    ?? throw new EntityNotFoundException(typeof(PersonalChat), chatId);

        personalChat.UpdateCompanionUnreadMessageCount(count);

        await _context.SaveChangesAsync();
    }
}
