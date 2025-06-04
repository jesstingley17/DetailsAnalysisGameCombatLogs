namespace CombatAnalysis.WebApp.Consts;

public class Authentication
{
    public string CookieDomain { get; set; }

    public string RedirectUri { get; set; }

    public string IdentityAuthPath { get; set; }

    public string IdentityRegistryPath { get; set; }

    public string CodeChallengeMethod { get; set; }

    public int RefreshTokenExpiresDays { get; set; }
}