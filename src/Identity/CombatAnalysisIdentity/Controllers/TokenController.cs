using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysisIdentity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TokenController(IOptions<AuthenticationGrantType> authenticationGrantType, IOptions<Authentication> authentication, IOAuthCodeFlowService oAuthCodeFlowService,
    ILogger<TokenController> logger) : ControllerBase
{
    private readonly AuthenticationGrantType _authenticationGrantType = authenticationGrantType.Value;
    private readonly Authentication _authentication = authentication.Value;
    private readonly IOAuthCodeFlowService _oAuthCodeFlowService = oAuthCodeFlowService;
    private readonly ILogger<TokenController> _logger = logger;

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutModel logout)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(logout, nameof(logout));

            var rowsAffected = await _oAuthCodeFlowService.RevokeRefreshTokenAsync(logout.RefreshTokenId);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Failed to revoke JWT. Parameter '{ParamName}' should be more then zero", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to revoke JWT. Parameter '{ParamName}' was null", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetJsonWebToken(string grantType, string clientId, string clientScopes, string codeVerifier, string code, string redirectUri)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(grantType, nameof(grantType));
            ArgumentException.ThrowIfNullOrEmpty(clientId, nameof(clientId));
            ArgumentException.ThrowIfNullOrEmpty(clientScopes, nameof(clientScopes));
            ArgumentException.ThrowIfNullOrEmpty(codeVerifier, nameof(codeVerifier));
            ArgumentException.ThrowIfNullOrEmpty(code, nameof(code));
            ArgumentException.ThrowIfNullOrEmpty(redirectUri, nameof(redirectUri));

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

            var token = await GenerateTokenAsync(userId, clientId, clientScopes);
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            return Ok(token);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Failed to get JWT. Paramter '{ParamName} was null", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshAccessToken(string grantType, string clientId, string clientScopes, string refreshTokenId, string refreshToken)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(grantType, nameof(grantType));
            ArgumentException.ThrowIfNullOrEmpty(clientId, nameof(clientId));
            ArgumentException.ThrowIfNullOrEmpty(clientScopes, nameof(clientScopes));
            ArgumentException.ThrowIfNullOrEmpty(refreshTokenId, nameof(refreshTokenId));
            ArgumentException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

            if (!grantType.Equals(_authenticationGrantType.RefreshToken))
            {
                return BadRequest();
            }

            var userId = await _oAuthCodeFlowService.ValidateRefreshTokenAsync(refreshTokenId, refreshToken, clientId);
            ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));

            var token = await RotateTokenAsync(userId, clientId, clientScopes, refreshTokenId);
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            return Ok(token);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to refresh JWT. Parameter '{ParamName}' was null", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Failed to refresh JWT. Parameter '{ParamName}' was incorrect", ex.ParamName);

            return BadRequest();
        }
    }

    private async Task<TokenResponseDto> GenerateTokenAsync(string userId, string clientId, string clientScopes)
    {
        var scopes = clientScopes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var accessToken = _oAuthCodeFlowService.GenerateToken(userId, clientId, scopes);
        var refreshToken = _oAuthCodeFlowService.GenerateRefreshToken();

        var refreshTokenId = await _oAuthCodeFlowService.CreateRefreshTokenAsync(refreshToken, _authentication.RefreshTokenExpiresDays, clientId, userId);

        var token = new TokenResponseDto
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            Expires = DateTimeOffset.UtcNow.AddMinutes(_authentication.AccessTokenExpiresMins),
            RefreshToken = new RefreshTokenResponseDto
            {
                Id = refreshTokenId,
                Token = refreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(_authentication.RefreshTokenExpiresDays)
            }
        };

        return token;
    }

    private async Task<TokenResponseDto> RotateTokenAsync(string userId, string clientId, string clientScopes, string oldRefreshTokenId)
    {
        var scopes = clientScopes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var accessToken = _oAuthCodeFlowService.GenerateToken(userId, clientId, scopes);
        var refreshToken = _oAuthCodeFlowService.GenerateRefreshToken();

        var newRefreshTokenId = await _oAuthCodeFlowService.CreateRefreshTokenAsync(refreshToken, _authentication.RefreshTokenExpiresDays, clientId, userId);
        await _oAuthCodeFlowService.RotateRefreshTokenAsync(oldRefreshTokenId, newRefreshTokenId);

        var token = new TokenResponseDto
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            Expires = DateTimeOffset.UtcNow.AddMinutes(_authentication.AccessTokenExpiresMins),
            RefreshToken = new RefreshTokenResponseDto
            {
                Id = newRefreshTokenId,
                Token = refreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(_authentication.RefreshTokenExpiresDays)
            }
        };

        return token;
    }
}
