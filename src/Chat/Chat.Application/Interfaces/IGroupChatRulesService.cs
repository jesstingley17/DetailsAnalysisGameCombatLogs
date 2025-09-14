using Chat.Application.DTOs;

namespace Chat.Application.Interfaces;

public interface IGroupChatRulesService
{
    Task<GroupChatRulesDto> CreateAsync(GroupChatRulesDto createRules);

    Task DeleteAsync(int chatId);

    Task<GroupChatRulesDto> GetByChatIdAsync(int chatId);

    Task UpdateAsync(GroupChatRulesDto updateRules);
}
