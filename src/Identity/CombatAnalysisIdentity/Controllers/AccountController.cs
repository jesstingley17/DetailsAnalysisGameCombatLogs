using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysisIdentity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountController(IUserAuthorizationService authorizationService, ILogger<AccountController> logger) : ControllerBase
{
    private readonly IUserAuthorizationService _authorizationService = authorizationService;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDataModel model)
    {
        if (!model.Password.Equals(model.ConfirmPassword))
        {
            _logger.LogError("Password and confirm password should be equal");
            ModelState.AddModelError(string.Empty, "Password and confirm password should be equal");

            return ValidationProblem(ModelState);
        }

        var isPresent = await _authorizationService.CheckIfIdentityUserPresentAsync(model.Email);
        if (isPresent)
        {
            _logger.LogError("User with this Email already present");
            ModelState.AddModelError(string.Empty, "User with this Email already present");

            return ValidationProblem(ModelState);
        }

        var usernameAlreadyUsed = await _authorizationService.CheckIfUsernameAlreadyUsedAsync(model.Username);
        if (usernameAlreadyUsed)
        {
            _logger.LogError("Username already used");
            ModelState.AddModelError(string.Empty, "Username already used");

            return ValidationProblem(ModelState);
        }

        var passwordIsStrong = _authorizationService.IsPasswordStrong(model.Password);
        if (!passwordIsStrong)
        {
            _logger.LogError("Password should have at least 8 characters, upper/lowercase character, digit and special symbol");
            ModelState.AddModelError(string.Empty, "Password should have at least 8 characters, upper/lowercase character, digit and special symbol");

            return ValidationProblem(ModelState);
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid RegistrationDataModel create received: {@RegistrationDataModel}", model);

            return ValidationProblem(ModelState);
        }

        var (hash, salt) = PasswordHashing.HashPasswordWithSalt(model.Password);
        var identityUser = new IdentityUserModel
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email,
            PasswordHash = hash,
            Salt = salt
        };

        var appUser = new AppUserModel
        {
            Id = Guid.NewGuid().ToString(),
            Username = model.Username,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            Birthday = model.Birthday,
            IdentityUserId = identityUser.Id
        };

        var customer = new CustomerModel
        {
            Id = Guid.NewGuid().ToString(),
            Country = model.Country,
            City = model.City,
            PostalCode = model.PostalCode,
            AppUserId = appUser.Id,
        };

        var wasCreated = await _authorizationService.CreateUserAsync(identityUser, appUser, customer);
        if (!wasCreated)
        {
            _logger.LogWarning("Some problems during Registration. Please, try one more time late");

            return BadRequest();
        }

        return Ok();
    }
}
