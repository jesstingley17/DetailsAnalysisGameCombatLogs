using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IIdentityUserRepository
{
    Task CreateAsync(IdentityUser identityUser);

    Task<int> UpdateAsync(string id, IdentityUser item);

    Task<IdentityUser?> GetByIdAsync(string id);

    Task<bool> CheckByEmailAsync(string email);

    Task<IdentityUser?> GetAsync(string email);
}
