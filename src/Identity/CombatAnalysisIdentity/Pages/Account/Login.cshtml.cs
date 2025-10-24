using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages.Account;

public class LoginModel(IUserAuthorizationService authorizationService) : PageModel
{
    private readonly IUserAuthorizationService _authorizationService = authorizationService;

    public string CancelRequestAddress { get; set; } = "cancel=true";

    [BindProperty]
    public AuthorizationDataModel? Authorization { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return Page();
        }

        if (Authorization == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return Page();
        }

        await _authorizationService.AuthorizationAsync(HttpContext, Authorization.Email, Authorization.Password);
        if (Request.Query.TryGetValue("ReturnUrl", out var returnUrl))
        {
            return Redirect(returnUrl.ToString());
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt");

        return Page();
    }
}
