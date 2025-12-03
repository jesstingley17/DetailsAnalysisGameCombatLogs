using Chat.Application.DTOs;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Interfaces;

public interface IGroupChatUserService : IService<GroupChatUserDto, string>
{
    Task UpdateChatUserAsync(GroupChatUserId id,
                                GroupChatMessageId? lastMessageId = null,
                                int? unreadMessages = null);

    Task<IEnumerable<GroupChatUserDto>> FindAllAsync(int chatId);

    Task<GroupChatUserDto> FindByAppUserIdAsync(int chatId, string appUserId);

    Task<IEnumerable<GroupChatUserDto>> FindAllByAppUserIdAsync(string appUserId);
}
