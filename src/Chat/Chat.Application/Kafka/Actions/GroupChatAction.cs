using Chat.Application.DTOs;
using Chat.Application.Enums;
using Chat.Application.Kafka.Security;

namespace Chat.Application.Kafka.Actions;

public class GroupChatAction : SecurityAction
{
    public GroupChatDto Chat { get; set; }

    public GroupChatRulesDto Rules { get; set; }

    public GroupChatUserDto User { get; set; }

    public ChatActionState State { get; set; }

    public DateTimeOffset When { get; set; }
}
