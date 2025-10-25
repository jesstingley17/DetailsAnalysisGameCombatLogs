using CombatAnalysis.Identity.Interfaces;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class NewPasswordModel(IUserAuthorizationService authorizationService, IUserVerification userVerification) : PageModel
{
    private readonly IUserAuthorizationService _authorizationService = authorizationService;
    private readonly IUserVerification _userVerification = userVerification;

    public string CancelRequestUri { get; private set; } = string.Empty;

    [BindProperty]
    public PasswordResetModel PasswordReset{ get; set; }

    public IActionResult OnGet(string token)
    {
        CancelRequestUri = Request.Query["redirectUri"]!;

        PasswordReset = new PasswordResetModel
        {
            Token = token,
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        CancelRequestUri = Request.Query["redirectUri"]!;

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
