using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Authorization;

[Route("api/v1/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly Authentication _authentication;
    private readonly AuthenticationGrantType _authenticationGrantType;
    private readonly AuthenticationClient _authenticationClient;
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(IOptions<Cluster> cluster, IOptions<Authentication> authentication, IOptions<AuthenticationGrantType> authenticationGrantType,
        IOptions<AuthenticationClient> authenticationClient, IHttpClientHelper httpClient, ILogger<IdentityController> logger)
    {
        _httpClient = httpClient;
        _authentication = authentication.Value;
        _authenticationGrantType = authenticationGrantType.Value;
        _authenticationClient = authenticationClient.Value;
        _logger = logger;
        _httpClient.APIUrl = cluster.Value.Identity;
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var refreshToken))
            {
                ArgumentException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));
            }

            var refreshTokenParts = refreshToken.Split(':', 2);
            var logout = new LogoutModel { RefreshTokenId = refreshTokenParts[0] };
            var responseMessage = await _httpClient.PostAsync("Token/logout", JsonContent.Create(logout));
            responseMessage.EnsureSuccessStatusCode();

            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.RefreshToken), new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });
            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.AccessToken), new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            return Ok();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Failed to logout. Paramter '{ParamName} was incorrect", ex.ParamName);

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to send HTTP Request to logout");

            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> AuthorizationCodeExchange(string authorizationCode)
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.CodeVerifier), out var codeVerifier))
            {
                ArgumentNullException.ThrowIfNullOrEmpty(codeVerifier, nameof(codeVerifier));
            }

            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.CodeVerifier), new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            var decodedAuthorizationCode = Uri.UnescapeDataString(authorizationCode);
            var url = $"Token?grantType={_authenticationGrantType.Authorization}" +
                $"&clientId={_authenticationClient.ClientId}" +
                $"&clientScopes={_authenticationClient.Scopes}" +
                $"&codeVerifier={codeVerifier}" +
                $"&code={decodedAuthorizationCode}" +
                $"&redirectUri={_authentication.RedirectUri}";

            var responseMessage = await _httpClient.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();

            var token = await responseMessage.Content.ReadFromJsonAsync<TokenResponseModel>();
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.AccessToken), token.AccessToken, new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = token.Expires,
            });
            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.RefreshToken), $"{token.RefreshToken.Id}:{token.RefreshToken.Token}", new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = token.Expires.AddDays(_authentication.RefreshTokenExpiresDays)
            });

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to exchange to the Authorization code. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to send HTTP Request to receive Authorization code");

            return BadRequest();
        }
    }

    [ServiceFilter(typeof(RequireRefreshTokenAttribute))]
    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshJWT()
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var refreshToken))
            {
                ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));
            }

            var refreshTokenParts = refreshToken.Split(':', 2);
            var responseMessage = await _httpClient.GetAsync($"Token/refresh?" +
                $"grantType={_authenticationGrantType.RefreshToken}" +
                $"&clientId={_authenticationClient.ClientId}" +
                $"&clientScopes={_authenticationClient.Scopes}" +
                $"&refreshTokenId={refreshTokenParts[0]}" +
                $"&refreshToken={refreshTokenParts[1]}");
            responseMessage.EnsureSuccessStatusCode();

            var token = await responseMessage.Content.ReadFromJsonAsync<TokenResponseModel>();
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.AccessToken), token.AccessToken, new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = token.Expires,
            });
            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.RefreshToken), $"{token.RefreshToken.Id}:{token.RefreshToken.Token}", new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = token.Expires.AddDays(_authentication.RefreshTokenExpiresDays)
            });

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to refresh JWT. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to refresh JWT");

            return BadRequest();
        }
    }

    [HttpGet("userPrivacy/{id}")]
    public async Task<IActionResult> GetUserPrivacy(string id)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(id, nameof(id));

            var responseMessage = await _httpClient.GetAsync($"Identity/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var userPrivacy = await responseMessage.Content.ReadFromJsonAsync<IdentityUserPrivacyModel>();
            ArgumentNullException.ThrowIfNull(userPrivacy, nameof(userPrivacy));

            return Ok(userPrivacy);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to get user privacy. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to send HTTP Request to receive user privacy");

            return BadRequest();
        }
    }
}
