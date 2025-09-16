namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public class GroupChatUserModel
{
    public string Id { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public int UnreadMessages { get; set; }

    public int GroupChatId { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
