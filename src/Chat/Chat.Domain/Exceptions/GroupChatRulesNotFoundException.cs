namespace Chat.Domain.Exceptions;

public class GroupChatRulesNotFoundException(int chatRulesId) : DomainException($"Chat rules with Id '{chatRulesId}' was not found.")
{
    public int ChatRulesId { get; } = chatRulesId;
}
