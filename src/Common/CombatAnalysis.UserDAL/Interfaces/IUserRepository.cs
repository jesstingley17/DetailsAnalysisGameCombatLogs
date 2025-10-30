using CombatAnalysis.UserDAL.Entities;

namespace CombatAnalysis.UserDAL.Interfaces;

public interface IUserRepository
{
    Task<AppUser> CreateAsync(AppUser item);

    Task<int> UpdateAsync(string id, AppUser item);

    Task<int> DeleteAsync(string id);

    Task<AppUser?> GetByIdAsync(string id);

    Task<IEnumerable<AppUser>> GetAllAsync();

    Task<AppUser?> FindByIdentityUserIdAsync(string identityUserId);

    Task<IEnumerable<AppUser>> FindByUsernameStartAtAsync(string identityUserId);
}
