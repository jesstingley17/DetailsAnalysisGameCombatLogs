using CombatAnalysis.Identity.Interfaces;
using CombatAnalysisIdentity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;

namespace CombatAnalysisIdentity.Pages;

public class InitVerificationModel(EmailService emailService, IUserVerification userVerification, ILogger<InitVerificationModel> logger) : PageModel
{
    private readonly IUserVerification _userVerification = userVerification;
    private readonly EmailService _emailService = emailService;
    private readonly ILogger<InitVerificationModel> _logger = logger;

    public async Task<IActionResult> OnGet(string email)
    {
        try
        {
            var token = await _userVerification.GenerateVerifyEmailTokenAsync(email);

            var redirectUri = Request.Query["redirectUri"];
            var verifyLink = $"{Request.Scheme}://{Request.Host}/verification?token={token}&redirectUri={redirectUri}";

            await SendVerifyEmailToEmailAsync(email, verifyLink);

            return Page();
        }
        catch (SmtpException ex)
        {
            ModelState.AddModelError(string.Empty, "Some problems during sending Verification link to email. Please, try one more time");

            var exMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            _logger.LogError(ex, exMessage);

            return Page();
        }
    }

    private async Task SendVerifyEmailToEmailAsync(string email, string verifyLink)
    {
        const string subject = "Email verification";
        string body = $"<p>Click on <a href=\"{verifyLink}\">Verify</a> for verification your email.</p>";

        await _emailService.SendResetPasswordEmailAsync(email, subject, body);
    }
}
