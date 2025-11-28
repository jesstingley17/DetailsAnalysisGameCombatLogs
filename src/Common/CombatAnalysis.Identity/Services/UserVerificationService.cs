using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Transactions;

namespace CombatAnalysis.Identity.Services;

internal class UserVerificationService(IResetTokenRepository resetTokenRepository, IVerifyEmailTokenRepository verifyEmailRepository, IIdentityUserService identityUserService,
    ILogger<UserVerificationService> logger) : IUserVerification
{
    private readonly IResetTokenRepository _resetTokenRepository = resetTokenRepository;
    private readonly IVerifyEmailTokenRepository _verifyEmailRepository = verifyEmailRepository;
    private readonly IIdentityUserService _identityUserService = identityUserService;
    private readonly ILogger<UserVerificationService> _logger = logger;

    public async Task<string> GenerateResetTokenAsync(string email)
    {
        var token = GenerateToken();

        var resetToken = new ResetToken
        {
            Email = email,
            Token = token,
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };

        await _resetTokenRepository.CreateAsync(resetToken);

        return token;
    }

    public async Task<string> GenerateVerifyEmailTokenAsync(string email)
    {
        var token = GenerateToken();

        var verifyEmailToken = new VerifyEmailToken
        {
            Email = email,
            Token = token,
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };

        await _verifyEmailRepository.CreateAsync(verifyEmailToken);

        return token;
    }

    public async Task<bool> ResetPasswordAsync(string token, string password)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            var resetToken = await _resetTokenRepository.GetByTokenAsync(token);
            ArgumentNullException.ThrowIfNull(resetToken, nameof(resetToken));
            ArgumentOutOfRangeException.ThrowIfEqual(resetToken.IsUsed, true, nameof(resetToken.IsUsed));
            ArgumentOutOfRangeException.ThrowIfLessThan(resetToken.ExpirationTime, DateTime.UtcNow, nameof(resetToken.ExpirationTime));

            var (hash, salt) = PasswordHashing.HashPasswordWithSalt(password);

            var identityUser = await _identityUserService.GetByEmailAsync(resetToken.Email);
            identityUser.PasswordHash = hash;
            identityUser.Salt = salt;

            await _identityUserService.UpdateAsync(identityUser.Id, identityUser);

            resetToken.IsUsed = true;
            await _resetTokenRepository.UpdateAsync(resetToken.Id, resetToken);

            scope.Complete();

            return true;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            scope.Dispose();

            return false;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, ex.Message);

            scope.Dispose();

            return false;
        }
        catch (Exception) 
        {
            scope.Dispose();

            return false;
        }
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var verifyToken = await _verifyEmailRepository.GetByTokenAsync(token);
        if (verifyToken == null || verifyToken.IsUsed || verifyToken.ExpirationTime < DateTime.UtcNow)
        {
            return false;
        }

        verifyToken.IsUsed = true;
        await _verifyEmailRepository.UpdateAsync(verifyToken.Id, verifyToken);

        var identityUser = await _identityUserService.GetByEmailAsync(verifyToken.Email);
        if (identityUser == null)
        {
            return false;
        }

        identityUser.EmailVerified = true;
        await _identityUserService.UpdateAsync(identityUser.Id, identityUser);

        scope.Complete();

        return true;
    }

    public async Task RemoveExpiredVerificationAsync()
    {
        await _verifyEmailRepository.RemoveExpiredVerifyEmailTokenAsync();
        await _resetTokenRepository.RemoveExpiredResetTokenAsync();
    }

    private static string GenerateToken()
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        var randomBytes = new byte[32]; // 256 bits
        randomNumberGenerator.GetBytes(randomBytes);

        var code = Convert.ToBase64String(randomBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");

        return code;
    }
}
