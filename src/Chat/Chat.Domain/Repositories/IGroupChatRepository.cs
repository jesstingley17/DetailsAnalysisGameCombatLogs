using Chat.Domain.Aggregates;
using Chat.Domain.Entities;

namespace Chat.Domain.Repositories;

public interface IGroupChatRepository
{
    Task<GroupChat?> CreateAsync(GroupChat item);

    Task UpdateAsync(GroupChat item);

    Task DeleteAsync(GroupChat item);

    Task<IEnumerable<GroupChat>> GetAllAsync();

    Task<GroupChat?> GetByIdAsync(int id);

    Task<IEnumerable<GroupChatMessage>> GetChatMessagesAsync(int chatId, int page, int pageSize);
}
