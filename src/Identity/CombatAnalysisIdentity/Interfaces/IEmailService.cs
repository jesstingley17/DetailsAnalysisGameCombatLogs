namespace CombatAnalysisIdentity.Interfaces;

public interface IEmailService
{
    Task SendResetPasswordEmailAsync(string email, string subject, string body, bool isBodyHtml = true);
}
