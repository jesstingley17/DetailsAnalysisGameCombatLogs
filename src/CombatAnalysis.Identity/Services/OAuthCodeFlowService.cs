using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CombatAnalysis.Identity.Services;

internal class OAuthCodeFlowService(IOptions<Authentication> authentication, IOptions<AuthenticationClient> authenticationClient, IPkeRepository pkeRepository,
    IClientRepository clientRepository) : IOAuthCodeFlowService
{
    private readonly Authentication _authentication = authentication.Value;
    private readonly AuthenticationClient _authenticationClient = authenticationClient.Value;
    private readonly IPkeRepository _pkeRepository = pkeRepository;
    private readonly IClientRepository _clientRepository = clientRepository;

    async Task<string> IOAuthCodeFlowService.GenerateAuthorizationCodeAsync(string userId, string clientId, string codeChallenge, string codeChallengeMethod, string redirectUri)
    {
        var authorizationCode = GenerateAuthorizationCode();

        var encryptedAuthorizationCode = EncryptAuthorizationCode(authorizationCode, userId, _authentication.IssuerSigningKey);

        await _pkeRepository.CreateAsync(clientId, encryptedAuthorizationCode, codeChallenge, codeChallengeMethod, redirectUri);

        return encryptedAuthorizationCode;
    }

    async Task<bool> IOAuthCodeFlowService.ValidateCodeChallengeAsync(string clientId, string codeVerifier, string authorizationCode, string redirectUri)
    {
        if (string.IsNullOrEmpty(clientId)
            || string.IsNullOrEmpty(codeVerifier)
            || string.IsNullOrEmpty(authorizationCode)
            || string.IsNullOrEmpty(redirectUri))
        {
            throw new ArgumentNullException("One of parameters is null.");
        }

        var authorizationCodeChallenge = await _pkeRepository.GetByIdAsync(authorizationCode);
        if (authorizationCodeChallenge == null)
        {
            throw new ArgumentException("Authorization code not found.");
        }

        if (authorizationCodeChallenge.IsUsed)
        {
            throw new ArgumentException("Authorization code is used already.");
        }

        var redirectUriIsValid = authorizationCodeChallenge.RedirectUrl == redirectUri;
        if (!redirectUriIsValid)
        {
            throw new ArgumentException("RedirectUrl incorrect.");
        }

        var clientIdIsValid = authorizationCodeChallenge.ClientId == clientId;
        if (!clientIdIsValid)
        {
            throw new ArgumentException("Client id invalidate.");
        }

        var generatedCodeChallenge = GenerateCodeChallenge(codeVerifier);
        var codeChallengeIsValid = authorizationCodeChallenge.CodeChallenge == generatedCodeChallenge;
        if (!codeChallengeIsValid)
        {
            throw new ArgumentException("Code challenge invalidate.");
        }

        await MarkCodeAsUsedAsync(authorizationCodeChallenge);

        return true;
    }

    async Task<bool> IOAuthCodeFlowService.ValidateClientAsync(string clientId, string redirectUri, string clientScopes, bool isDevRequest)
    {
        if (string.IsNullOrEmpty(clientId)
            || string.IsNullOrEmpty(clientScopes)
            || string.IsNullOrEmpty(redirectUri))
        {
            return false;
        }

        var client = await _clientRepository.GetByIdAsync(clientId);
        if (client == null)
        {
            return false;
        }

        var allowedRedirects = client.RedirectUrl.Split(';');
        if (allowedRedirects.Length == 0)
        {
            return false;
        }

        var redirectUriIsValid = allowedRedirects.Contains(redirectUri);
        if (!redirectUriIsValid && !isDevRequest)
        {
            return false;
        }

        var scopeIsValid = client.AllowedScopes == clientScopes;

        return scopeIsValid;
    }

    (string AuthorizationCode, string UserData) IOAuthCodeFlowService.DecryptAuthorizationCode(string encryptedDataWithCustomData, byte[] encryptionKey)
    {
        var fullCipher = Convert.FromBase64String(encryptedDataWithCustomData);

        using var aes = Aes.Create();
        aes.Key = encryptionKey;

        var iv = new byte[aes.BlockSize / 8];
        var cipher = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        aes.IV = iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        var decryptedData = sr.ReadToEnd();

        var parts = decryptedData.Split([':'], 2);

        var decryptedAuthorizationKey = (parts[0], parts.Length > 1 ? parts[1] : string.Empty);

        return decryptedAuthorizationKey;
    }

    string IOAuthCodeFlowService.GenerateToken(string userId, string clientId, string[] scopes)
    {
        var key = new SymmetricSecurityKey(_authentication.IssuerSigningKey);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;

        ArgumentNullException.ThrowIfNull(now, nameof(now));
        ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));
        ArgumentNullException.ThrowIfNull(scopes, nameof(scopes));
        ArgumentException.ThrowIfNullOrEmpty(clientId, nameof(clientId));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                  new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Sub, userId),
            new("scope", string.Join(",", scopes)),
            new("client_id", clientId),
        };

        var audiences = _authenticationClient.Audiences.Split(',');
        var payload = new JwtPayload(
            issuer: _authentication.Issuer,
            audience: null,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_authentication.AccessTokenExpiresMins)
        )
        {
            ["aud"] = audiences
        };

        var jwySecurityToken = new JwtSecurityToken(new JwtHeader(creds), payload);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(jwySecurityToken);

        return token;
    }

    string IOAuthCodeFlowService.GenerateRefreshToken()
    {
        var randomNumber = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(randomNumber)
                                .Replace('+', '-')
                                .Replace('/', '_');

        return token;
    }

    private static string EncryptAuthorizationCode(string authorizationCode, string customData, byte[] encryptionKey)
    {
        string combinedData = $"{authorizationCode}:{customData}";

        using var aes = Aes.Create();
        aes.Key = encryptionKey;
        aes.GenerateIV();

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(combinedData);
        }

        var iv = aes.IV;
        var encrypted = ms.ToArray();

        var result = new byte[iv.Length + encrypted.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

        var encryptedAuthorizationKey = Convert.ToBase64String(result);

        return encryptedAuthorizationKey;
    }

    private static string GenerateAuthorizationCode()
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        var randomBytes = new byte[32];
        randomNumberGenerator.GetBytes(randomBytes);

        var code = Convert.ToBase64String(randomBytes);
        return code;
    }

    private static string GenerateCodeChallenge(string verifier)
    {
        var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(verifier));

        var codeChallenge = Convert.ToBase64String(challengeBytes)
                      .TrimEnd('=')
                      .Replace('+', '-')
                      .Replace('/', '_');

        return codeChallenge;
    }

    private async Task<int> MarkCodeAsUsedAsync(AuthorizationCodeChallenge authorizationCodeChallenge)
    {
        authorizationCodeChallenge.IsUsed = true;

        var affectedRows = await _pkeRepository.MarkCodeAsUsedAsync(authorizationCodeChallenge);

        return affectedRows;
    }
}
