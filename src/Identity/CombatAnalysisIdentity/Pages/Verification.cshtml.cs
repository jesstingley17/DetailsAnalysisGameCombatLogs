using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CombatAnalysisIdentity.Pages;

public class VerificationModel(IOptions<Authentication> authentication, IUserVerification userVerification, ILogger<VerificationModel> logger) : PageModel
{
    private readonly Authentication _authentication = authentication.Value;
    private readonly IUserVerification _userVerification = userVerification;
    private readonly ILogger<VerificationModel> _logger = logger;

    public async Task<IActionResult> OnGetAsync(string token)
    {
        try
        {
            var wasVerified = await _userVerification.VerifyEmailAsync(token);
            if (wasVerified)
            {
                var redirectUri = $"{_authentication.Protocol}://{Request.Query["redirectUri"]}?verified=true";

                return Redirect(redirectUri);
            }

            return Page();
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError(string.Empty, "Some problems during email verification. Please, try one more time");

            var exMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            _logger.LogError(ex, exMessage);

            return Page();
        }
    }
}
