namespace CombatAnalysis.EnhancedWebApp.Server.Models.Identity;

public class RefreshTokenResponseModel
{
    public string Id { get; set; }

    public string Token { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }
}
