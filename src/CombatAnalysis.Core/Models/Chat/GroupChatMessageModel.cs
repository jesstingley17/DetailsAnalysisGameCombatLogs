namespace CombatAnalysis.Core.Models.Chat;

public class GroupChatMessageModel
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTimeOffset Time { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public int MarkedType { get; set; }

    public bool IsEdited { get; set; }

    public int ChatId { get; set; }

    public string GroupChatUserId { get; set; } = string.Empty;

    public int? GroupChatMessageId { get; set; }
}
