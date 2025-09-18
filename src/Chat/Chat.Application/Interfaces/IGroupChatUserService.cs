using Chat.Application.DTOs;

namespace Chat.Application.Interfaces;

public interface IGroupChatUserService : IService<GroupChatUserDto, string>
{
    Task MarkAsReadAsync(string groupChatUserId, int chatMessageId);

    Task<bool> IsAllUsersReadMessageAsync(int chatId, string messageOwnerId, int messageId);

    Task<IEnumerable<GroupChatUserDto>> FindAllAsync(int chatId);

    Task<GroupChatUserDto> FindByAppUserIdAsync(int chatId, string appUserId);

    Task<IEnumerable<GroupChatUserDto>> FindAllByAppUserIdAsync(string appUserId);
}
