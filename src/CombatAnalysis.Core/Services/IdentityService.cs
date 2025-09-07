using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Identity;
using CombatAnalysis.Core.Security;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Services;

internal class IdentityService(IMemoryCache memoryCache, IHttpClientHelper httpClient, ILogger logger) : IIdentityService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IHttpClientHelper _httpClient = httpClient;
    private readonly ILogger _logger = logger;
    private readonly SecurityStorage _securityStorage = new(memoryCache, httpClient, logger);

    private string? _codeVerifier;
    private string? _code;
    private HttpListenerService? _httpListenerService;

    public async Task SendAuthorizationRequestAsync(string authorizationRequestType)
    {
        _codeVerifier = PKCEHelper.GenerateCodeVerifier();
        var state = PKCEHelper.GenerateCodeVerifier();
        var codeChallenge = PKCEHelper.GenerateCodeChallenge(_codeVerifier);

        var authorizationUrl = $"{API.Identity}{authorizationRequestType}?" +
            $"grantType={AuthenticationGrantType.Code}" +
            $"&clientId={Authentication.ClientId}" +
            $"&redirectUri={Authentication.RedirectUri}" +
            $"&scopes={Authentication.Scopes}" +
            $"&state={state}" +
            "&codeChallengeMethod=SHA-256" +
            $"&codeChallenge={codeChallenge}";

        var psi = new ProcessStartInfo
        {
            FileName = authorizationUrl,
            UseShellExecute = true,
        };
        Process.Start(psi);

        _httpListenerService = new HttpListenerService(Authentication.Listener, _logger);
        await _httpListenerService.StartListeningAsync(OnCallbackReceived);
    }

    public async Task SendTokenRequestAsync()
    {
        try
        {
            var token = await GetTokenAsync();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            await SetMemoryCacheAsync(token.RefreshToken.Token, token.AccessToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCallbackReceived(string authorizationCode, string incomingState)
    {
        _code = authorizationCode;
    }

    private async Task<TokenResponseModel> GetTokenAsync()
    {
        try
        {
            if (_code == null)
            {
                throw new ArgumentNullException(nameof(_code));
            }

            var encodedAuthorizationCode = Uri.EscapeDataString(_code);
            var url = $"Token?" +
                $"grantType={AuthenticationGrantType.Authorization}" +
                $"&clientId={Authentication.ClientId}" +
                $"&clientScopes={Authentication.Scopes}" +
                $"&codeVerifier={_codeVerifier}" +
                $"&code={encodedAuthorizationCode}" +
                $"&redirectUri={Authentication.RedirectUri}";

            var response = await _httpClient.GetAsync(url, API.Identity);
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<TokenResponseModel>();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new TokenResponseModel();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new TokenResponseModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return new TokenResponseModel();
        }
    }

    private async Task SetMemoryCacheAsync(string refreshToken, string aceessToken)
    {
        _securityStorage.SaveTokens(refreshToken, aceessToken);
        var user = await _securityStorage.GetUserAsync();

        _memoryCache.Set(nameof(MemoryCacheValue.RefreshToken), refreshToken, new MemoryCacheEntryOptions { Size = 10 });
        _memoryCache.Set(nameof(MemoryCacheValue.AccessToken), aceessToken, new MemoryCacheEntryOptions { Size = 10 });
        _memoryCache.Set(nameof(MemoryCacheValue.User), user, new MemoryCacheEntryOptions { Size = 50 });
    }
}
