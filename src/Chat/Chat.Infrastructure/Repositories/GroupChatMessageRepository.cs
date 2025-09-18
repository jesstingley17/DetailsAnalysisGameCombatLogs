using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatMessageRepository(ChatContext context) : GenericRepository<GroupChatMessage, GroupChatMessageId>(context), IGroupChatMessageRepository
{
    public async Task UpdateAsync(GroupChatMessage updated)
    {
        var groupChatMessage = await GetByIdAsync(updated.Id)
                    ?? throw new EntityNotFoundException(typeof(GroupChatMessage), updated.Id);

        groupChatMessage.ApplyUpdates(updated);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GroupChatMessage>> GetByChatIdAsync(int chatId, int page, int pageSize)
    {
        var messages = await _context.GroupChatMessage
                            .AsNoTracking()
                            .Where(m => m.GroupChatId == chatId)
                            .OrderBy(m => m.Time)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return messages;
    }

    public async Task ReadMessagesLessThanAsync(int chatId, int messageId)
    {
        var messages = await _context.GroupChatMessage
                            .Where(m => m.GroupChatId == chatId
                                        && m.Type == MessageType.Default
                                        && m.Id <= messageId)
                            .ToListAsync();

        foreach (var item in messages)
        {
            item.UpdateStatus(MessageStatus.Read);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<int> CountReadUnreadMessagesAsync(int chatId, int chatMessageId, int lastReadMessageId)
    {
        var countReadUnreadMessages = await _context.GroupChatMessage
                                            .AsNoTracking()
                                            .Where(m => m.GroupChatId == chatId 
                                                        && m.Type == MessageType.Default)
                                            .CountAsync(m => m.Id > lastReadMessageId && m.Id <= chatMessageId);

        return countReadUnreadMessages;
    }

    public async Task<int> CountReadUnreadMessagesAsync(int chatId, int chatMessageId)
    {
        var countReadUnreadMessages = await _context.GroupChatMessage
                                            .AsNoTracking()
                                            .Where(m => m.GroupChatId == chatId
                                                        && m.Type == MessageType.Default)
                                            .CountAsync(m => m.Id <= chatMessageId);

        return countReadUnreadMessages;
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _context.GroupChatMessage
                     .CountAsync(c => c.GroupChatId == chatId);

        return count;
    }
}
