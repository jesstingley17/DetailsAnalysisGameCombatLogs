using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IGroupChatUserRepository : IGenericRepository<GroupChatUser, GroupChatUserId>
{
    Task MarkAsReadAsyn(string groupChatUserId, int chatMessageId);

    Task<bool> IsAllUsersReadMessageAsync(int chatId, string messageOwnerId, int messageId);

    Task UpdateAsync(GroupChatUser updated);

    Task<IEnumerable<GroupChatUser>> FindAllAsync(int chatId);

    Task<IEnumerable<GroupChatUser>> FindAllByAppUserIdAsync(string appUserId);

    Task<GroupChatUser?> FindByAppUserIdAsync(int chatId, string appUserId);
}
