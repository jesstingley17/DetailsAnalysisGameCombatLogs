namespace CombatAnalysis.Identity.Security;

public class Authentication
{
    public string Issuer { get; set; }

    public int AccessTokenExpiresMins { get; set; }

    public int RefreshTokenExpiresDays { get; set; }
}