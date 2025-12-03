using Chat.Application.DTOs;
using Chat.Application.Enums;
using Chat.Application.Kafka.Security;

namespace Chat.Application.Kafka.Actions;

public class GroupChatMemberAction : SecurityAction
{
    public GroupChatUserDto User { get; set; }

    public string ChatOwnerId { get; set; } = string.Empty;

    public ChatMembersActionState State { get; set; }

    public DateTimeOffset When { get; set; }
}
