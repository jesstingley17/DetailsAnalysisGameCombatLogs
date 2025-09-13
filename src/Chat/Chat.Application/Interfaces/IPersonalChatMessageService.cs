using Chat.Application.DTOs;

namespace Chat.Application.Interfaces;

public interface IPersonalChatMessageService<TIdType> : IService<PersonalChatMessageDto, TIdType>
    where TIdType : notnull
{
    Task<IEnumerable<PersonalChatMessageDto>> GetByChatIdAsync(int chatId, int pageSize);

    Task<IEnumerable<PersonalChatMessageDto>> GetMoreByChatIdAsync(int chatId, int offset, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}
