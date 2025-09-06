using AutoMapper;
using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using CombatAnalysisIdentity.Security;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CombatAnalysisIdentity.Services;

internal class UserAuthorizationService(IMapper mapper, IOAuthCodeFlowService oAuthCodeFlowService, IOptions<Authentication> authentication, 
    IOptions<API> api, IIdentityUserService identityUserService, ILogger<UserAuthorizationService> logger,
    IUserService<AppUserDto> appUserService, IService<CustomerDto, string> customerService, ICustomerTransactionService customerTransactionService, 
    IIdentityTransactionService identityTransactionService) : IUserAuthorizationService
{
    private readonly IMapper _mapper = mapper;
    private readonly IOAuthCodeFlowService _oAuthCodeFlowService = oAuthCodeFlowService;
    private readonly IIdentityUserService _identityUserService = identityUserService;
    private readonly Authentication _authentication = authentication.Value;
    private readonly API _api = api.Value;
    private readonly IUserService<AppUserDto> _appUserService = appUserService;
    private readonly IService<CustomerDto, string> _customerService = customerService;
    private readonly ICustomerTransactionService _userTransactionService = customerTransactionService;
    private readonly IIdentityTransactionService _identityTransactionService = identityTransactionService;
    private readonly ILogger<UserAuthorizationService> _logger = logger;
    private readonly AuthorizationRequestModel _authorizationRequest = new();

    async Task<string> IUserAuthorizationService.AuthorizationAsync(HttpRequest request, string email, string password)
    {
        var user = await _identityUserService.GetByEmailAsync(email);
        if (user == null)
        {
            return string.Empty;
        }

        var passwordIsValid = PasswordHashing.VerifyPassword(password, user.PasswordHash, user.Salt);
        if (!passwordIsValid)
        {
            return string.Empty;
        }

        GetAuthorizationRequestData(request);

        var authorizationCode = await _oAuthCodeFlowService.GenerateAuthorizationCodeAsync(user.Id, _authorizationRequest.ClientTd, _authorizationRequest.CodeChallenge, _authorizationRequest.CodeChallengeMethod, _authorizationRequest.RedirectUri);

        var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
        var redirectUrl = $"{_authentication.Protocol}://{_authorizationRequest.RedirectUri}?code={encodedAuthorizationCode}&state={_authorizationRequest.State}";

        return redirectUrl;
    }

    async Task<bool> IUserAuthorizationService.ClientValidationAsync(HttpRequest request, bool isDevRequest = false)
    {
        GetAuthorizationRequestData(request);

        var clientIsValid = await _oAuthCodeFlowService.ValidateClientAsync(_authorizationRequest.ClientTd, _authorizationRequest.RedirectUri, _authorizationRequest.Scopes, isDevRequest);

        return clientIsValid;
    }

    async Task<bool> IUserAuthorizationService.CreateUserAsync(IdentityUserModel identityUser, AppUserModel appUser, CustomerModel customer)
    {
        try
        {
            await _userTransactionService.BeginTransactionAsync();
            await _identityTransactionService.BeginTransactionAsync();

            var identityUserMap = _mapper.Map<IdentityUserDto>(identityUser);
            await _identityUserService.CreateAsync(identityUserMap);

            var appUserMap = _mapper.Map<AppUserDto>(appUser);
            await _appUserService.CreateAsync(appUserMap);

            var customerMap = _mapper.Map<CustomerDto>(customer);
            await _customerService.CreateAsync(customerMap);

            await _userTransactionService.CommitTransactionAsync();
            await _identityTransactionService.CommitTransactionAsync();

            return true;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex.Message);

            await _userTransactionService.RollbackTransactionAsync();
            await _identityTransactionService.RollbackTransactionAsync();

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            await _userTransactionService.RollbackTransactionAsync();
            await _identityTransactionService.RollbackTransactionAsync();

            return false;
        }
    }

    async Task<bool> IUserAuthorizationService.CheckIfIdentityUserPresentAsync(string email)
    {
        var userPresent = await _identityUserService.CheckByEmailAsync(email);

        return userPresent;
    }

    async Task<bool> IUserAuthorizationService.CheckIfUsernameAlreadyUsedAsync(string username)
    {
        try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{_api.User}api/v1/Account/check/{username}");
            response.EnsureSuccessStatusCode();

            var usernameAlreadyUsed = await response.Content.ReadFromJsonAsync<bool>();
            return usernameAlreadyUsed;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return false;
        }
    }

    bool IUserAuthorizationService.IsPasswordStrong(string password)
    {
        // Check if the password is at least 8 characters long
        if (password.Length < 8)
        {
            return false;
        }

        // Check if the password contains at least one uppercase letter
        if (!Regex.IsMatch(password, "[A-Z]"))
        {
            return false;
        }

        // Check if the password contains at least one lowercase letter
        if (!Regex.IsMatch(password, "[a-z]"))
        {
            return false;
        }

        // Check if the password contains at least one digit
        if (!Regex.IsMatch(password, "[0-9]"))
        {
            return false;
        }

        // Check if the password contains at least one special character
        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
        {
            return false;
        }

        return true;
    }

    private void GetAuthorizationRequestData(HttpRequest request)
    {
        if (request.Query.TryGetValue(AuthorizationRequest.RedirectUri.ToString(), out var redirectUri))
        {
            _authorizationRequest.RedirectUri = redirectUri;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.GrantType.ToString(), out var grantType))
        {
            _authorizationRequest.GrantType = grantType;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.ClientId.ToString(), out var clientId))
        {
            _authorizationRequest.ClientTd = clientId;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.Scopes.ToString(), out var scopes))
        {
            _authorizationRequest.Scopes = scopes;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.State.ToString(), out var state))
        {
            _authorizationRequest.State = state;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.CodeChallengeMethod.ToString(), out var codeChallengeMethod))
        {
            _authorizationRequest.CodeChallengeMethod = codeChallengeMethod;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.CodeChallenge.ToString(), out var codeChallenge))
        {
            _authorizationRequest.CodeChallenge = codeChallenge;
        }
    }
    
    private void GetDevAuthorizationRequestData(HttpRequest request)
    {
        var returnUrlEncoded = request.Query["ReturnUrl"];
        var returnUrlDecoded = Uri.UnescapeDataString(returnUrlEncoded);
        var innerQuery = new Uri("https://localhost" + returnUrlDecoded);
        var innerParams = Microsoft.AspNetCore.WebUtilities
                .QueryHelpers.ParseQuery(innerQuery.Query);

        if (innerParams.TryGetValue("redirect_uri", out var redirectUri))
        {
            var url = redirectUri.ToString();
            var isHttps = url.StartsWith("https://");
            _authorizationRequest.RedirectUri = isHttps ? url.Split("https://")[1] : url.Split("http://")[1];
        }

        if (innerParams.TryGetValue("response_type", out var grantType))
        {
            _authorizationRequest.GrantType = grantType;
        }

        if (innerParams.TryGetValue("client_id", out var clientId))
        {
            _authorizationRequest.ClientTd = clientId;
        }

        if (innerParams.TryGetValue(AuthorizationRequest.Scopes.ToString(), out var scope))
        {
            _authorizationRequest.Scopes = scope;
        }

        if (innerParams.TryGetValue(AuthorizationRequest.State.ToString(), out var state))
        {
            _authorizationRequest.State = state;
        }

        if (innerParams.TryGetValue("code_challenge_method", out var codeChallengeMethod))
        {
            _authorizationRequest.CodeChallengeMethod = codeChallengeMethod;
        }

        if (innerParams.TryGetValue("code_challenge", out var codeChallenge))
        {
            _authorizationRequest.CodeChallenge = codeChallenge;
        }
    }
}
