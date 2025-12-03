using Chat.Domain.Enums;

namespace Chat.Domain.Exceptions;

public class GroupChatRulesNotFoundException(int chatRulesId) : DomainException($"Chat rules with Id '{chatRulesId}' was not found.", ExceptionCode.NotFound)
{
    public int ChatRulesId { get; } = chatRulesId;
}
