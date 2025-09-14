using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatUserRepository(ChatContext context) : GenericRepository<GroupChatUser, GroupChatUserId>(context), IGroupChatUserRepository
{
    public async Task UpdateAsync(GroupChatUser updated)
    {
        var groupChatUsers = await GetByIdAsync(updated.Id)
                    ?? throw new EntityNotFoundException(typeof(GroupChatUser), updated.Id);

        groupChatUsers.ApplyUpdates(updated);

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
