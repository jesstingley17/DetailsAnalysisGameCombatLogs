using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysisIdentity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TokenController(IOptions<AuthenticationGrantType> authenticationGrantType, IOptions<Authentication> authentication, IOAuthCodeFlowService oAuthCodeFlowService, ILogger<TokenController> logger) : ControllerBase
{
    private readonly AuthenticationGrantType _authenticationGrantType = authenticationGrantType.Value;
    private readonly Authentication _authentication = authentication.Value;
    private readonly IOAuthCodeFlowService _oAuthCodeFlowService = oAuthCodeFlowService;
    private readonly ILogger<TokenController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetJsonWebToken(string grantType, string clientId, string codeVerifier, string code, string redirectUri)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(grantType, nameof(grantType));
            ArgumentNullException.ThrowIfNullOrEmpty(clientId, nameof(clientId));
            ArgumentNullException.ThrowIfNullOrEmpty(codeVerifier, nameof(codeVerifier));
            ArgumentNullException.ThrowIfNullOrEmpty(code, nameof(code));
            ArgumentNullException.ThrowIfNullOrEmpty(redirectUri, nameof(redirectUri));

            if (!grantType.Equals(_authenticationGrantType.Authorization))
            {
                return BadRequest();
            }

            var decodedAuthorizationCode = Uri.UnescapeDataString(code).Replace(' ', '+');
            var codeChallengeValidated = await _oAuthCodeFlowService.ValidateCodeChallengeAsync(clientId, codeVerifier, decodedAuthorizationCode, redirectUri);
            if (!codeChallengeValidated)
            {
                return BadRequest();
            }

            var (authorizationCode, userId) = _oAuthCodeFlowService.DecryptAuthorizationCode(decodedAuthorizationCode, _authentication.IssuerSigningKey);

            var token = await GenerateTokenAsync(clientId, userId);
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            return Ok(token);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to get JWT. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshAccessToken(string grantType, string refreshToken, string clientId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(grantType, nameof(grantType));
            ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));
            ArgumentNullException.ThrowIfNullOrEmpty(clientId, nameof(clientId));

            if (!grantType.Equals(_authenticationGrantType.RefreshToken))
            {
                return BadRequest();
            }

            var userId = await _oAuthCodeFlowService.ValidateRefreshTokenAsync(refreshToken, clientId);
            ArgumentNullException.ThrowIfNullOrEmpty(userId, nameof(userId));

            var token = GenerateNewTokenWhenRefresh(clientId, userId, refreshToken);
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            return Ok(token);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to refresh JWT. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
    }

    private async Task<AccessTokenDto> GenerateTokenAsync(string clientId, string userId)
    {
        var accessToken = _oAuthCodeFlowService.GenerateToken(clientId, userId);
        var refreshToken = _oAuthCodeFlowService.GenerateToken(clientId);

        await _oAuthCodeFlowService.SaveRefreshTokenAsync(refreshToken, _authentication.RefreshTokenExpiresDays, clientId, userId);

        var token = new AccessTokenDto
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            Expires = DateTimeOffset.UtcNow.AddMinutes(_authentication.AccessTokenExpiresMins),
            RefreshToken = refreshToken
        };

        return token;
    }

    private AccessTokenDto GenerateNewTokenWhenRefresh(string clientId, string userId, string refreshToken)
    {
        var accessToken = _oAuthCodeFlowService.GenerateToken(clientId, userId);

        var token = new AccessTokenDto
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            Expires = DateTimeOffset.UtcNow.AddMinutes(_authentication.AccessTokenExpiresMins),
            RefreshToken = refreshToken
        };

        return token;
    }
}
