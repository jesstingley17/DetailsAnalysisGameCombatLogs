using Chat.Domain.Enums;

namespace CombatAnalysis.Hubs.Models;

public class GroupChatMessageModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public DateTimeOffset Time { get; set; }

    public MessageStatus Status { get; set; }

    public MessageType Type { get; set; }

    public MessageMarkedType MarkedType { get; set; }

    public bool IsEdited { get; set; }

    public int GroupChatId { get; set; }

    public string GroupChatUserId { get; set; }
}
