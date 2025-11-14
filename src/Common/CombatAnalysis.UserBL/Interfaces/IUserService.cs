using CombatAnalysis.UserBL.DTO;

namespace CombatAnalysis.UserBL.Interfaces;

public interface IUserService
{
    Task<AppUserDto?> CreateAsync(AppUserDto item);

    Task UpdateAsync(string id, AppUserDto item);

    Task DeleteAsync(string id);

    Task<IEnumerable<AppUserDto>> GetAllAsync();

    Task<AppUserDto?> GetByIdAsync(string id);

    Task<bool> CheckByUsernameAsync(string username);

    Task<AppUserDto?> FindByIdentityUserIdAsync(string identityUserId);

    Task<IEnumerable<AppUserDto>> FindByUsernameStartAtAsync(string startAt);
}
