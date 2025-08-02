namespace CombatAnalysis.EnhancedWebApp.Server.Models.User;

public class RequestToConnectModel
{
    public int Id { get; set; }

    public string ToAppUserId { get; set; } = string.Empty;

    public DateTimeOffset When { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
