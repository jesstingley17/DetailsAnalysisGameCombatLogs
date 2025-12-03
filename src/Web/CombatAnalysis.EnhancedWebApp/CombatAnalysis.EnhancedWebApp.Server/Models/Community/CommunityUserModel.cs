namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public class CommunityUserModel
{
    public string Id { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string AppUserId { get; set; } = string.Empty;

    public int CommunityId { get; set; }
}
