using Chat.Domain.DTOs;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatMessageRepository(ChatContext context) : GenericRepository<GroupChatMessage, GroupChatMessageId>(context), IGroupChatMessageRepository
{
    public async Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsync(int chatId, int page, int pageSize)
    {
        var messages = await (
            from m in _context.Set<GroupChatMessage>()
            join gu in _context.Set<GroupChatUser>() on m.GroupChatUserId equals gu.Id
            select new GroupChatMessageDto(
                m.Id,
                m.Username,
                m.Message,
                m.Time,
                m.Status,
                m.Type,
                m.MarkedType,
                m.IsEdited,
                m.GroupChatId,
                m.GroupChatUserId,
                gu.AppUserId
            )
        )
        .Skip(page * pageSize)
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
