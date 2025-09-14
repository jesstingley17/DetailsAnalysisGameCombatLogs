namespace Chat.Domain.Exceptions;

public class GroupChatUserNotFoundException(string userId) : DomainException($"Chat user with Id '{userId}' was not found.")
{
    public string UserId { get; } = userId;
}
