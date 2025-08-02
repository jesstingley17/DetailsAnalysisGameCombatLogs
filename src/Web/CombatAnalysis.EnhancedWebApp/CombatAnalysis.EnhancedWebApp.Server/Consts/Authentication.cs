namespace CombatAnalysis.EnhancedWebApp.Server.Consts;

public class Authentication
{
    public string CookieDomain { get; set; } = string.Empty;

    public string RedirectUri { get; set; } = string.Empty;

    public string IdentityAuthPath { get; set; } = string.Empty;

    public string IdentityRegistryPath { get; set; } = string.Empty;

    public string CodeChallengeMethod { get; set; } = string.Empty;

    public int RefreshTokenExpiresDays { get; set; }
}