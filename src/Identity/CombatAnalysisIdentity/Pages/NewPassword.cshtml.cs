using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace CombatAnalysisIdentity.Pages;

public class NewPasswordModel(IOptions<Authentication> authentication, IUserAuthorizationService authorizationService, IUserVerification userVerification) : PageModel
{
    private readonly IUserAuthorizationService _authorizationService = authorizationService;
    private readonly IUserVerification _userVerification = userVerification;

    [BindProperty]
    public PasswordResetModel PasswordReset{ get; set; }

    public IActionResult OnGet(string token)
    {
        PasswordReset = new PasswordResetModel
        {
            Token = token,
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Confirm password should be equal Password");

            return Page();
        }

        var passwordIsStrong = _authorizationService.IsPasswordStrong(PasswordReset.Password);
        if (!passwordIsStrong)
        {
            ModelState.AddModelError(string.Empty, "Password should have at least 8 characters, upper/lowercase character, digit and special symbol");

            return Page();
        }

        var wasReseted = await _userVerification.ResetPasswordAsync(PasswordReset.Token, PasswordReset.Password);
        if (wasReseted)
        {
            var redirectUri = $"{Request.Query["redirectUri"]}?accessRestored=true";

            return Redirect(redirectUri);
        }

        return Page();
    }
}
