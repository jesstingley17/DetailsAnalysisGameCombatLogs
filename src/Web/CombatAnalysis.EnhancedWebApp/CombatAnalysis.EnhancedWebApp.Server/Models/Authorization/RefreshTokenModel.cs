namespace CombatAnalysis.EnhancedWebApp.Server.Models.Authorization;

public class RefreshTokenModel
{
    public string Id { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public DateTimeOffset Expires { get; set; }
}
