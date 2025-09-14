using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IGroupChatRepository : IGenericRepository<GroupChat, GroupChatId>
{
    Task UpdateAsync(GroupChat updated);

    Task<GroupChatRules?> AddRulesAsync(GroupChatRules rules);

    Task RemoveRulesAsync(int chatId);

    Task UpdateRulesAsync(GroupChatRules updateRules);
}