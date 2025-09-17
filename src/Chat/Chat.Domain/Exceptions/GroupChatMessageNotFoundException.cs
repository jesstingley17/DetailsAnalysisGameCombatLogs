using Chat.Domain.Enums;

namespace Chat.Domain.Exceptions;

public class GroupChatMessageNotFoundException(int messageId) : DomainException($"Group chat message with Id '{messageId}' was not found.", ExceptionCode.NotFound)
{
    public int MessageId { get; } = messageId;
}
