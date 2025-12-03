using Chat.Domain.Enums;

namespace Chat.Domain.Exceptions;

public class GroupChatNotFoundException(int groupChatId) : DomainException($"Group chat with Id '{groupChatId}' was not found.", ExceptionCode.NotFound)
{
    public int GroupChatId { get; } = groupChatId;
}