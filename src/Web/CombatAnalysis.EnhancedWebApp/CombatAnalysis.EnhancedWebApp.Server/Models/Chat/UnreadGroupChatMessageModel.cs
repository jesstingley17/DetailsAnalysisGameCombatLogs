namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public class UnreadGroupChatMessageModel
{
    public int Id { get; set; }

    public string GroupChatUserId { get; set; } = string.Empty;

    public int GroupChatMessageId { get; set; }
}
