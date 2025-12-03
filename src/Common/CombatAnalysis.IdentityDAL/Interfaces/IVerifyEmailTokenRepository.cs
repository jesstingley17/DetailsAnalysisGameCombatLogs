using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IVerifyEmailTokenRepository
{
    Task CreateAsync(VerifyEmailToken verifyCode);

    Task<int> UpdateAsync(int id, VerifyEmailToken verifyCode);

    Task<VerifyEmailToken> GetByIdAsync(int id);

    Task<VerifyEmailToken> GetByTokenAsync(string token);

    Task RemoveExpiredVerifyEmailTokenAsync();
}
