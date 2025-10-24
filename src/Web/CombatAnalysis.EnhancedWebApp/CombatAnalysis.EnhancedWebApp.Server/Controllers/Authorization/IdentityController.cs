using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Authorization;

[Route("api/v1/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly Authentication _authentication;
    private readonly AuthenticationClient _authenticationClient;
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<IdentityController> _logger;
    private readonly string _apiUrl;

    public IdentityController(IOptions<Cluster> cluster, IOptions<Authentication> authentication,
        IOptions<AuthenticationClient> authenticationClient, IHttpClientHelper httpClient, ILogger<IdentityController> logger)
    {
        _httpClient = httpClient;
        _authentication = authentication.Value;
        _authenticationClient = authenticationClient.Value;
        _logger = logger;
        _httpClient.APIUrl = cluster.Value.Identity;
        _apiUrl = cluster.Value.Identity;
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        try
        {
            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.RefreshToken));
            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.AccessToken));
            HttpContext.Response.Cookies.Delete("idsrv.session");

            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = "/"
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
            );
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

            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.CodeVerifier));

            using var client = new HttpClient();

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = _authenticationClient.ClientId,
                ["code"] = authorizationCode,
                ["redirect_uri"] = _authentication.RedirectUri,
                ["code_verifier"] = codeVerifier
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl}connect/token")
            {
                Content = new FormUrlEncodedContent(form)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<TokenResponseModel>();
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.AccessToken), token.AccessToken, new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn),
            });
            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.RefreshToken), token.RefreshToken, new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddSeconds(_authentication.RefreshTokenExpiresSec),
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

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshJWT()
    {
        try
        {
            if (!HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var refreshToken))
            {
                ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));
            }

            using var client = new HttpClient();

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["client_id"] = _authenticationClient.ClientId,
                ["refresh_token"] = refreshToken,
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl}connect/token")
            {
                Content = new FormUrlEncodedContent(form)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<TokenResponseModel>();
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.AccessToken), token.AccessToken, new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn),
            });
            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.RefreshToken), token.RefreshToken, new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddSeconds(_authentication.RefreshTokenExpiresSec),
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

            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.RefreshToken));
            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.AccessToken));

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
