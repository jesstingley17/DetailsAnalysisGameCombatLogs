using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using System.Net.Http.Headers;

namespace CombatAnalysis.Hubs.Helpers;

internal class HttpClientHelper(IHttpContextAccessor httpContextAccessor) : IHttpClientHelper
{
    private const string _baseAddressApi = "api/v1/";

    private readonly HttpClient _client = new();
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string APIUrl { get; set; } = string.Empty;

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
    {
        AddAuthorizationHeader();

        var responseMessage = await _client.PostAsync($"{APIUrl}{_baseAddressApi}{requestUri}", content);

        return responseMessage;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        AddAuthorizationHeader();

        var responseMessage = await _client.GetAsync($"{APIUrl}{_baseAddressApi}{requestUri}");

        return responseMessage;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        AddAuthorizationHeader();

        var responseMessage = await _client.PutAsync($"{APIUrl}{_baseAddressApi}{requestUri}", content);

        return responseMessage;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        AddAuthorizationHeader();

        var responseMessage = await _client.DeleteAsync($"{APIUrl}{_baseAddressApi}{requestUri}");

        return responseMessage;
    }

    private void AddAuthorizationHeader()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (!context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var _))
        {
            throw new UnauthorizedAccessException($"{nameof(AuthenticationCookie.RefreshToken)} token is missing.");
        }

        if (!context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken))
        {
            throw new UnauthorizedAccessException($"{nameof(AuthenticationCookie.AccessToken)} token is missing.");
        }

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }
}
