namespace CombatAnalysis.Identity.Security;

public class Authentication
{
    public byte[] IssuerSigningKey { get; set; }

    public string Issuer { get; set; }

    public int AccessTokenExpiresMins { get; set; }

    public int RefreshTokenExpiresDays { get; set; }

    public string Protocol { get; set; }
}