using Chat.Application.DTOs;

namespace Chat.Application.Interfaces;

public interface IGroupChatService : IService<GroupChatDto, int>
{
    Task AddRulesAsync(GroupChatRulesDto item);

    Task UpdateRulesAsync(GroupChatRulesDto item);

    Task<GroupChatRulesDto> GetRulesAsync(int chatId);
}
