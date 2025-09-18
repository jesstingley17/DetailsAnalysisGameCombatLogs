using Chat.Application.DTOs;

namespace Chat.Application.Interfaces;

public interface IPersonalChatService : IService<PersonalChatDto, int>
{
    Task<IEnumerable<PersonalChatDto>> GetByUserIdAsync(string userId);
}
