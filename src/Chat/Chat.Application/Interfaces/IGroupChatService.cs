using Chat.Application.DTOs;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Interfaces;

public interface IGroupChatService : IService<GroupChatDto, int>
{
    Task UpdateChatAsync(GroupChatId id,
                                     string? newName,
                                     string? newOwnerId);

    Task<GroupChatRulesDto> AddRulesAsync(GroupChatRulesDto item);

    Task UpdateRulesAsync(GroupChatRulesDto item);

    Task<GroupChatRulesDto> GetRulesAsync(int chatId);
}
