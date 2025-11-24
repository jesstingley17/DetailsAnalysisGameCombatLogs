using Chat.Domain.Aggregates;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class PersonalChatRepository(ChatContext context) : GenericRepository<PersonalChat, PersonalChatId>(context), IPersonalChatRepository
{
    public async Task<IEnumerable<PersonalChat>> GetByUserIdAsync(string userId)
    {
        var chats = await context.PersonalChat
            .AsNoTracking()
            .Where(x => x.InitiatorId == userId || x.CompanionId == userId)
            .ToListAsync();

        return chats;
    }

    public async Task UpdateInitiatorUnreadMessageCountAsync(int chatId, int count)
    {
        var entity = await _context.Set<PersonalChat>()
                 .SingleOrDefaultAsync(g => g.Id == chatId)
                        ?? throw new EntityNotFoundException(typeof(PersonalChat), chatId);

        entity.UpdateInitiatorUnreadMessageCount(count);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateCompanionUnreadMessageCountAsync(int chatId, int count)
    {
        var entity = await _context.Set<PersonalChat>()
                 .SingleOrDefaultAsync(g => g.Id == chatId)
                        ?? throw new EntityNotFoundException(typeof(PersonalChat), chatId);

        entity.UpdateCompanionUnreadMessageCount(count);

        await _context.SaveChangesAsync();
    }
}
