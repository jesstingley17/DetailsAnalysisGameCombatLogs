using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace CombatAnalysisIdentity.Pages.Account;

public class LoginModel(IUserAuthorizationService authorizationService) : PageModel
{
    private readonly IUserAuthorizationService _authorizationService = authorizationService;

    public string CancelRequestAddress { get; private set; } = "cancel=true";

    public string CancelRequestUri { get; private set; } = string.Empty;

    [BindProperty]
    public AuthorizationDataModel? Authorization { get; set; }

    public IActionResult OnGet()
    {
        var nestedParams = HttpUtility.ParseQueryString(new Uri("http://dummy" + Request.Query["ReturnUrl"]).Query);

        string cancelUri = HttpUtility.UrlDecode(nestedParams["cancel_uri"]!);

        CancelRequestUri = cancelUri;

        return Page();
    }

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
        Request.Query.TryGetValue("client_id", out var clientId);

        if (string.Equals(clientId, Clients.Web, StringComparison.OrdinalIgnoreCase) && Request.Query.TryGetValue("ReturnUrl", out var returnUrl))
        {
            return Redirect(returnUrl.ToString());
        }
        else if (string.Equals(clientId, Clients.Desktop, StringComparison.OrdinalIgnoreCase))
        {
            return Redirect($"/connect/authorize/callback{Request.QueryString}");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt");

        return Page();
    }
}
