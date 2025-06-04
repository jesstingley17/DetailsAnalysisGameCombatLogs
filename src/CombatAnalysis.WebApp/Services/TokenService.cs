using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Identity;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.WebApp.Services;

internal class TokenService : ITokenService
{
    private readonly AuthenticationGrantType _authenticationGrantType;
    private readonly AuthenticationClient _authenticationClient;
    private readonly IHttpClientHelper _httpClient;

    public TokenService(IOptions<Cluster> cluster, IOptions<AuthenticationGrantType> authenticationGrantType, IOptions<AuthenticationClient> authenticationClient, 
        IHttpClientHelper httpClient)
    {
        _authenticationGrantType = authenticationGrantType.Value;
        _authenticationClient = authenticationClient.Value;
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Identity;
    }

    public async Task<AccessTokenModel> RefreshAccessTokenAsync(string refreshToken)
    {
        var response = await _httpClient.GetAsync($"Token/refresh?grantType={_authenticationGrantType.RefreshToken}&refreshToken={refreshToken}&clientId={_authenticationClient.ClientId}");
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadFromJsonAsync<AccessTokenModel>();

            return token;
        }
        else
        {
            return null;
        }
    }

    public bool IsAccessTokenCloseToExpiry(string accessToken)
    {
        var validTo = AccessTokenHelper.GetValidToFromToken(accessToken);
        var isCloseToExpiry = DateTime.UtcNow.AddMinutes(5) >= validTo;

        return isCloseToExpiry;
    }
}
