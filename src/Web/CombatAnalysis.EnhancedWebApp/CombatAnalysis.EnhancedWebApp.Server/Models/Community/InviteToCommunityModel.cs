namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public class InviteToCommunityModel
{
    public int Id { get; set; }

    public int CommunityId { get; set; }

    public string ToAppUserId { get; set; } = string.Empty;

    public DateTimeOffset When { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
