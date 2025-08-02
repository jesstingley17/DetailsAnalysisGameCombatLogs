namespace CombatAnalysis.EnhancedWebApp.Server.Models.Identity;

public class AccessTokenModel
{
    public string AccessToken { get; set; } = string.Empty;

    public string TokenType { get; set; } = string.Empty;

    public DateTimeOffset Expires { get; set; }

    public string RefreshToken { get; set; } = string.Empty;
}
