namespace CombatAnalysisIdentity.Consts;

public class SmtpSettings
{
    public string Host { get; set; }

    public int Port { get; set; }

    public bool EnableSsl { get; set; }

    public bool UseDefaultCredentials { get; set; }

    public string DisplayName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}