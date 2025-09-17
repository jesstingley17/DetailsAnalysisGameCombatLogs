using Chat.Domain.Enums;

namespace Chat.Domain.Exceptions;

public class GroupChatUserNotFoundException(string userId) : DomainException($"Chat user with Id '{userId}' was not found.", ExceptionCode.NotFound)
{
    public string UserId { get; } = userId;
}
