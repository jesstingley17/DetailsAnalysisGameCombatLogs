namespace Chat.Domain.Exceptions;

public class PersonalChatNotFoundException(int personalChatId) : DomainException($"Personal chat with Id '{personalChatId}' was not found.")
{
    public int PersonalChatId { get; } = personalChatId;
}
