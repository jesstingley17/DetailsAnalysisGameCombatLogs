using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class PersonalChatMessageRepository(ChatContext context) : GenericRepository<PersonalChatMessage, PersonalChatMessageId>(context), IPersonalChatMessageRepository
{
    public async Task<IEnumerable<PersonalChatMessage>> GetByChatIdAsync(int chatId, int page, int pageSize)
    {
        var messages = await _context.PersonalChatMessage
                            .AsNoTracking()
                            .Where(m => m.PersonalChatId == chatId)
                            .OrderBy(m => m.Time)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return messages;
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _context.PersonalChatMessage
                     .CountAsync(c => c.PersonalChatId == chatId);

        return count;
    }
}
