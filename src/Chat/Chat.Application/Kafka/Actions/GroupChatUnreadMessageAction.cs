using Chat.Application.Enums;
using Chat.Application.Kafka.Security;

namespace Chat.Application.Kafka.Actions;

public class GroupChatUnreadMessageAction : SecurityAction
{
    public int ChatId { get; set; }

    public int MessageId { get; set; }

    public string GroupChatUserId { get; set; } = string.Empty;

    public ChatMessageActionState State { get; set; }
}
