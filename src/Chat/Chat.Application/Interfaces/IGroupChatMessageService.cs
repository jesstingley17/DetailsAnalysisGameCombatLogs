using Chat.Application.DTOs;

namespace Chat.Application.Interfaces;

public interface IGroupChatMessageService<TIdType> : IService<GroupChatMessageDto, TIdType>
    where TIdType : notnull
{
    Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsync(int chatId, string groupChatUserId, int pageSize);

    Task<IEnumerable<GroupChatMessageDto>> GetMoreByChatIdAsync(int chatId, string groupChatUserId, int offset, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}