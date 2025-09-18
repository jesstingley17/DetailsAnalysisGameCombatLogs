using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatUserRepository(ChatContext context) : GenericRepository<GroupChatUser, GroupChatUserId>(context), IGroupChatUserRepository
{
    public async Task MarkAsReadAsyn(string groupChatUserId, int chatMessageId)
    {
        var groupChatUser = await GetByIdAsync(groupChatUserId)
                    ?? throw new EntityNotFoundException(typeof(GroupChatUser), groupChatUserId);

        groupChatUser.MarkAsRead(chatMessageId);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsAllUsersReadMessageAsync(int chatId, string messageOwnerId, int messageId)
    {
        var chatUsersCount = await _context.GroupChatUser
                            .AsNoTracking()
                            .CountAsync(user => user.GroupChatId == chatId);

        var usersWhoReadCount = await _context.GroupChatUser
                            .AsNoTracking()
                            .Where(user => user.GroupChatId == chatId)
                            .CountAsync(user => user.LastReadMessageId != null && user.LastReadMessageId >= messageId);

        var allUsersReadMessage = (chatUsersCount - usersWhoReadCount - 1) <= 0;

        return allUsersReadMessage;
    }

    public async Task UpdateAsync(GroupChatUser updated)
    {
        var groupChatUser = await GetByIdAsync(updated.Id)
                    ?? throw new EntityNotFoundException(typeof(GroupChatUser), updated.Id);

        groupChatUser.ApplyUpdates(updated);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GroupChatUser>> FindAllAsync(int chatId)
    {
        var users = await _context.GroupChatUser
                            .AsNoTracking()
                            .Where(user => user.GroupChatId == chatId)
                            .ToListAsync();

        return users;
    }

    public async Task<IEnumerable<GroupChatUser>> FindAllByAppUserIdAsync(string appUserId)
    {
        var users = await _context.GroupChatUser
                            .AsNoTracking()
                            .Where(user => user.AppUserId == appUserId)
                            .ToListAsync();

        return users;
    }

    public async Task<GroupChatUser?> FindByAppUserIdAsync(int chatId, string appUserId)
    {
        var user = await _context.GroupChatUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(user => user.AppUserId == appUserId && user.GroupChatId == chatId);

        return user;
    }
}
