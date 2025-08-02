using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Helpers;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Authorization;
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
    public async Task<IActionResult> RefreshAccessToken()
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var identityUserId = AccessTokenHelper.GetUserIdFromToken(accessToken);
            var response = await _httpClient.GetAsync($"Account/find/{identityUserId}");
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<AppUserModel>();
            return Ok(user);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Authentication refresh was failed.");

            return BadRequest($"Authentication refresh was failed. Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Authentication refresh was failed.");

            return BadRequest($"Authentication refresh was failed. Error: {ex.Message}");
        }
    }

    [HttpGet("authorization")]
    public IActionResult Authorization(string identityPath)
    {
        var codeVerifier = PKCEHelper.GenerateCodeVerifier();
        var state = PKCEHelper.GenerateCodeVerifier();
        var codeChallenge = PKCEHelper.GenerateCodeChallenge(codeVerifier);

        var uri = $"{_server.Identity}{identityPath}?grantType={_authenticationGrantType.Code}" +
            $"&clientId={_authenticationClient.ClientId}&redirectUri={_authentication.RedirectUri}" +
            $"&scope={_authenticationClient.Scope}&state={state}&codeChallengeMethod={_authentication.CodeChallengeMethod}" +
            $"&codeChallenge={codeChallenge}";

        var identityRedirect = new IdentityRedirect
        {
            Uri = uri
        };

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

        return Ok(identityRedirect);
    }

    [HttpGet("verifyEmail")]
    public IActionResult VerifyEmail(string identityPath, string email)
    {
        var uri = $"{_server.Identity}{identityPath}?email={email}&redirectUri={_authentication.RedirectUri}";

        var identityRedirect = new IdentityRedirect
        {
            Uri = uri
        };

        return Ok(identityRedirect);
    }

    [HttpGet("stateValidate")]
    public IActionResult StateValidate(string state)
    {
        if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.State), out var stateValue))
        {
            return BadRequest();
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
}
