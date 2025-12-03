using CombatAnalysis.Identity.Interfaces;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using System.Web;

namespace CombatAnalysisIdentity.Pages;

public class RestoreModel(IEmailService emailService, IUserAuthorizationService authorizationService, IUserVerification userVerification, 
    ILogger<RestoreModel> logger) : PageModel
{
    private readonly IEmailService _emailService = emailService;
    private readonly IUserAuthorizationService _authorizationService = authorizationService;
    private readonly IUserVerification _userVerification = userVerification;
    private readonly ILogger<RestoreModel> _logger = logger;

    public int SendEmailRespond { get; private set; }

    public string CancelRequestUri { get; private set; } = string.Empty;

    [BindProperty]
    public RestoreDataModel Restore { get; set; } = new RestoreDataModel();

    public IActionResult OnGet()
    {
        ExtractCancelRequestUri();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            ExtractCancelRequestUri();

            var isPresent = await _authorizationService.CheckIfIdentityUserPresentAsync(Restore.Email);
            if (!isPresent)
            {
                ModelState.AddModelError(string.Empty, "User with this Email not present");

                return Page();
            }

            var token = await _userVerification.GenerateResetTokenAsync(Restore.Email);

            var resetLink = $"{Request.Scheme}://{Request.Host}/newPassword?token={token}&redirectUri={CancelRequestUri}";

            await SendResetPasswordToEmailAsync(Restore.Email, resetLink);

            SendEmailRespond = 1;

            return Page();
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "Error sending email");

            ModelState.AddModelError(string.Empty, "Error sending email. Please, try one more time later");

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Page();
        }
    }

    private async Task SendResetPasswordToEmailAsync(string email, string resetLink)
    {
        const string subject = "Password Reset";
        string body = $"<p>Click on <a href=\"{resetLink}\">Restore link</a> to reset your password.</p>";

        await _emailService.SendResetPasswordEmailAsync(email, subject, body);
    }

    private void ExtractCancelRequestUri()
    {
        CancelRequestUri = Request.Query["cancel_uri"]!;
        if (CancelRequestUri == null)
        {
            var nestedParams = HttpUtility.ParseQueryString(new Uri("http://dummy" + Request.Query["ReturnUrl"]).Query);

            string cancelUri = HttpUtility.UrlDecode(nestedParams["cancel_uri"]!);

            CancelRequestUri = cancelUri;
        }
    }
}
