namespace Chat.Domain.Exceptions;

public class PersonalChatMessageNotFoundException(int messageId) : DomainException($"Perosnal chat message with Id '{messageId}' was not found.")
{
    public int MessageId { get; } = messageId;
}
