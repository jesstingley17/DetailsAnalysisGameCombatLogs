using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace CombatAnalysisIdentity.Pages.Account;

public class RegistrationModel(IUserAuthorizationService authorizationService) : PageModel
{
    private readonly IUserAuthorizationService _authorizationService = authorizationService;
    private IdentityUserModel? _identityUser;
    private AppUserModel? _appUser;
    private CustomerModel? _customer;

    public string CancelRequestUri { get; private set; } = string.Empty;

    [BindProperty]
    public RegistrationDataModel Registration { get; set; }

    public IActionResult OnGet()
    {
        ExtractCancelRequestUri();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ExtractCancelRequestUri();

        if (!Registration.Password.Equals(Registration.ConfirmPassword))
        {
            ModelState.AddModelError(string.Empty, "Password and confirm password should be equal");

            return Page();
        }

        var isPresent = await _authorizationService.CheckIfIdentityUserPresentAsync(Registration.Email);
        if (isPresent)
        {
            ModelState.AddModelError(string.Empty, "User with this Email already present");

            return Page();
        }

        var usernameAlreadyUsed = await _authorizationService.CheckIfUsernameAlreadyUsedAsync(Registration.Username);
        if (usernameAlreadyUsed)
        {
            ModelState.AddModelError(string.Empty, "Username already used");

            return Page();
        }

        var passwordIsStrong = _authorizationService.IsPasswordStrong(Registration.Password);
        if (!passwordIsStrong)
        {
            ModelState.AddModelError(string.Empty, "Password should have at least 8 characters, upper/lowercase character, digit and special symbol");

            return Page();
        }

        if (!ModelState.IsValid)
        {
            foreach (var modelStateValue in ModelState.Values) 
            {
                if (modelStateValue.ValidationState == ModelValidationState.Invalid)
                {
                    ModelState.AddModelError(string.Empty, modelStateValue.Errors[0].ErrorMessage);
                }
            }

            return Page();
        }

        FillIdentityUser();
        FillAppUser();
        FillCustomer();

        return await CreateUserAsync();
    }

    private void FillIdentityUser()
    {
        var (hash, salt) = PasswordHashing.HashPasswordWithSalt(Registration.Password);
        _identityUser = new IdentityUserModel
        {
            Id = Guid.NewGuid().ToString(),
            Email = Registration.Email,
            PasswordHash = hash,
            Salt = salt
        };
    }

    private void FillAppUser()
    {
        _appUser = new AppUserModel
        {
            Id = Guid.NewGuid().ToString(),
            Username = Registration.Username,
            FirstName = Registration.FirstName,
            LastName = Registration.LastName,
            PhoneNumber = Registration.PhoneNumber,
            Birthday = Registration.Birthday,
            IdentityUserId = _identityUser.Id
        };
    }

    private void FillCustomer()
    {
        _customer = new CustomerModel
        {
            Id = Guid.NewGuid().ToString(),
            Country = Registration.Country,
            City = Registration.City,
            PostalCode = Registration.PostalCode,
            AppUserId = _appUser.Id,
        };
    }

    private async Task<IActionResult> CreateUserAsync()
    {
        var wasCreated = await _authorizationService.CreateUserAsync(_identityUser, _appUser, _customer);
        if (!wasCreated)
        {
            ModelState.AddModelError(string.Empty, "Some problems during Registration. Please, try one more time late");

            return Page();
        }

        return Redirect(CancelRequestUri);
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
