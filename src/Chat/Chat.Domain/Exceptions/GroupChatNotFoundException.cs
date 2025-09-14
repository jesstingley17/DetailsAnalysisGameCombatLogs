namespace Chat.Domain.Exceptions;

public class GroupChatNotFoundException(int groupChatId) : DomainException($"Group chat with Id '{groupChatId}' was not found.")
{
    public int GroupChatId { get; } = groupChatId;
}