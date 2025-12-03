using Chat.Application.DTOs;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Interfaces;

public interface IPersonalChatService : IService<PersonalChatDto, int>
{
    Task UpdateChatAsync(PersonalChatId id,
                                         int? initiatorUnreadMessages,
                                         int? companionUnreadMessages);

    Task<IEnumerable<PersonalChatDto>> GetByUserIdAsync(string userId);
}
