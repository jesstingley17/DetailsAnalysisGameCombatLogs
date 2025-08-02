namespace CombatAnalysis.EnhancedWebApp.Server.Consts;

public class AuthenticationGrantType
{
    public string Code { get; set; } = string.Empty;

    public string Authorization { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}
