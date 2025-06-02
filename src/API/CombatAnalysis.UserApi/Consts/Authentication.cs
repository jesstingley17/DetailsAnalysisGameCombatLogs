namespace CombatAnalysis.UserApi.Consts;

internal class Authentication
{
    public byte[] IssuerSigningKey { get; set; }

    public string Issuer { get; set; }

    public string Authority { get; set; }
}
