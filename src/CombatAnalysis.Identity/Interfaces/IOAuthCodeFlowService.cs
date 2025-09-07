namespace CombatAnalysis.Identity.Interfaces;

public interface IOAuthCodeFlowService
{
    Task<string> GenerateAuthorizationCodeAsync(string userId, string clientId, string codeChallenge, string codeChallengeMethod, string redirectUri);

    Task<bool> ValidateCodeChallengeAsync(string clientId, string codeVerifier, string authorizationCode, string redirectUri);

    Task<bool> ValidateClientAsync(string clientId, string redirectUri, string clientScope, bool isDevRequest);

    (string AuthorizationCode, string UserData) DecryptAuthorizationCode(string encryptedDataWithCustomData, byte[] encryptionKey);

    Task<string> CreateRefreshTokenAsync(string token, int refreshTokenExpiresDays, string clientId, string userId);

    Task<int> RotateRefreshTokenAsync(string oldRefreshTokenId, string newRefreshTokenId);

    Task<int> RevokeRefreshTokenAsync(string refreshTokenId);

    string GenerateToken(string userId, string clientId, string[] scopes);

    string GenerateRefreshToken();

    Task<string> ValidateRefreshTokenAsync(string refreshTokenId,string refreshToken, string clientId);
}
