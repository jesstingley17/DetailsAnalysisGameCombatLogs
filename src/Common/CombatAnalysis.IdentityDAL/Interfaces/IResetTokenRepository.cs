using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IResetTokenRepository
{
    Task CreateAsync(ResetToken resetCode);

    Task<int> UpdateAsync(int id, ResetToken resetCode);

    Task<ResetToken> GetByIdAsync(int id);

    Task<ResetToken> GetByTokenAsync(string token);

    Task RemoveExpiredResetTokenAsync();
}
