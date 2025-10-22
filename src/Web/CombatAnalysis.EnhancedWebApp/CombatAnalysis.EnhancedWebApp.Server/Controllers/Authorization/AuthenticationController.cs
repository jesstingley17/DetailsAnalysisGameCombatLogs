using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Helpers;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Authorization;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly Authentication _authentication;
    private readonly AuthenticationGrantType _authenticationGrantType;
    private readonly AuthenticationClient _authenticationClient;
    private readonly Consts.Server _server;
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IOptions<Cluster> cluster, IOptions<Authentication> authentication, IOptions<AuthenticationGrantType> authenticationGrantType,
        IOptions<AuthenticationClient> authenticationClient, IOptions<Consts.Server> server, IHttpClientHelper httpClient,
        ILogger<AuthenticationController> logger)
    {
        _authentication = authentication.Value;
        _authenticationGrantType = authenticationGrantType.Value;
        _authenticationClient = authenticationClient.Value;
        _server = server.Value;
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.APIUrl = cluster.Value.User;
    }

    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    [HttpGet]
    public async Task<IActionResult> GetUserFromAccessToken()
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken))
            {
                ArgumentNullException.ThrowIfNullOrEmpty(accessToken, nameof(accessToken));
            }

            var identityUserId = AccessTokenHelper.GetUserIdFromAccessToken(accessToken);
            ArgumentNullException.ThrowIfNullOrEmpty(identityUserId, nameof(identityUserId));

            var responseMessage = await _httpClient.GetAsync($"User/find/{identityUserId}");
            responseMessage.EnsureSuccessStatusCode();

            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Ok(user);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to refresh Authentication token. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to refresh Authentication token");

            return BadRequest();
        }
    }

    [HttpGet("authorization")]
    public IActionResult Authorization(string identityPath)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(identityPath, nameof(identityPath));

            var codeVerifier = PKCEHelper.GenerateCodeVerifier();
            ArgumentNullException.ThrowIfNullOrEmpty(codeVerifier, nameof(codeVerifier));

            var state = PKCEHelper.GenerateCodeVerifier();
            ArgumentNullException.ThrowIfNullOrEmpty(state, nameof(state));

            var codeChallenge = PKCEHelper.GenerateCodeChallenge(codeVerifier);
            ArgumentNullException.ThrowIfNullOrEmpty(codeChallenge, nameof(codeChallenge));

            var uri = $"{_server.Identity}{identityPath}?" +
                $"grantType={_authenticationGrantType.Code}" +
                $"&clientId={_authenticationClient.ClientId}" +
                $"&redirectUri={_authentication.RedirectUri}" +
                $"&scopes={_authenticationClient.Scopes}" +
                $"&state={state}" +
                $"&codeChallengeMethod={_authentication.CodeChallengeMethod}" +
                $"&codeChallenge={codeChallenge}";

            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.CodeVerifier), codeVerifier, new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });
            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.State), state, new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            return Ok(new { uri });
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to Authorize. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("cancel")]
    public IActionResult CancelAuthorization()
    {
        HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.State), new CookieOptions
        {
            Domain = _authentication.CookieDomain,
            Path = "/",
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });

        HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.CodeVerifier), new CookieOptions
        {
            Domain = _authentication.CookieDomain,
            Path = "/",
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });

        return Ok();
    }

    [HttpGet("verifyEmail")]
    public IActionResult VerifyEmail(string identityPath, string email)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(identityPath, nameof(identityPath));
            ArgumentNullException.ThrowIfNullOrEmpty(email, nameof(email));

            var uri = $"{_server.Identity}{identityPath}?email={email}&redirectUri={_authentication.RedirectUri}";

            return Ok(new { uri });
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to Authorize. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("stateValidate")]
    public IActionResult StateValidate(string state)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(state, nameof(state));

            if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.State), out var stateValue))
            {
                ArgumentException.ThrowIfNullOrEmpty(stateValue, nameof(stateValue));
            }

            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.State), new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            if (stateValue == state)
            {
                return Ok();
            }

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to State validate. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Failed to State validate. Paramter '{ParamName} was failed", ex.ParamName);

            return BadRequest();
        }
    }
}
