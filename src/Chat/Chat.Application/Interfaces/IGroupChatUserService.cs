using Chat.Application.DTOs;

namespace Chat.Application.Interfaces;

public interface IGroupChatUserService : IService<GroupChatUserDto, string>
{
    Task<IEnumerable<GroupChatUserDto>> FindAllAsync(int chatId);

    Task<GroupChatUserDto> FindByAppUserIdAsync(int chatId, string appUserId);

    Task<IEnumerable<GroupChatUserDto>> FindAllByAppUserIdAsync(string appUserId);
}
