namespace CombatAnalysis.Hubs.Models;

public class GroupChatUserModel
{
    public string? Id { get; set; }

    public string Username { get; set; }

    public int UnreadMessages { get; set; }

    public int GroupChatId { get; set; }

    public string AppUserId { get; set; }
}
