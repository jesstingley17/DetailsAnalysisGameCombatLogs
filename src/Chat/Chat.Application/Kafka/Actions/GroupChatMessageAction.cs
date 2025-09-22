using Chat.Application.DTOs;
using Chat.Application.Enums;
using Chat.Application.Kafka.Security;

namespace Chat.Application.Kafka.Actions;

public class GroupChatMessageAction : SecurityAction
{
    public string InitiatorGroupChatUserId { get; set; } = string.Empty;

    public GroupChatMessageDto ChatMessage { get; set; }

    public ChatMessageActionState State { get; set; }

    public DateTimeOffset When { get; set; }
}
