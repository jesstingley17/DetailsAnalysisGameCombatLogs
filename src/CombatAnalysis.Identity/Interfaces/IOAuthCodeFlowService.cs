namespace CombatAnalysis.Identity.Interfaces;

public interface IOAuthCodeFlowService
{
    Task<string> GenerateAuthorizationCodeAsync(string userId, string clientId, string codeChallenge, string codeChallengeMethod, string redirectUri);

    Task<bool> ValidateCodeChallengeAsync(string clientId, string codeVerifier, string authorizationCode, string redirectUri);

    Task<bool> ValidateClientAsync(string clientId, string redirectUri, string clientScope, bool isDevRequest);

    (string AuthorizationCode, string UserData) DecryptAuthorizationCode(string encryptedDataWithCustomData, byte[] encryptionKey);

    string GenerateToken(string userId, string clientId, string[] scopes);

    string GenerateRefreshToken();
}
