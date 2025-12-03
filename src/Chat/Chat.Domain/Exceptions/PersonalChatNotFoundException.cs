using Chat.Domain.Enums;

namespace Chat.Domain.Exceptions;

public class PersonalChatNotFoundException(int personalChatId) : DomainException($"Personal chat with Id '{personalChatId}' was not found.", ExceptionCode.NotFound)
{
    public int PersonalChatId { get; } = personalChatId;
}
