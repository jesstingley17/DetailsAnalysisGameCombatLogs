using CombatAnalysisIdentity.Consts;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CombatAnalysisIdentity.Services;

public class EmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendResetPasswordEmailAsync(string email, string subject, string body, bool isBodyHtml = true)
    {
        var fromAddress = new MailAddress(_smtpSettings.Email, _smtpSettings.DisplayName);
        var toAddress = new MailAddress(email);

        var smtp = new SmtpClient
        {
            Host = _smtpSettings.Host,
            Port = _smtpSettings.Port,
            EnableSsl = _smtpSettings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = _smtpSettings.UseDefaultCredentials,
            Credentials = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password)
        };

        using var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = isBodyHtml,
        };

        await smtp.SendMailAsync(message);
    }
}
