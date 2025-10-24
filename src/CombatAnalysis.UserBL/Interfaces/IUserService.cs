using CombatAnalysis.UserBL.DTO;

namespace CombatAnalysis.UserBL.Interfaces;

public interface IUserService<TModel>
    where TModel : class
{
    Task<TModel?> CreateAsync(TModel item);

    Task UpdateAsync(TModel item);

    Task DeleteAsync(TModel item);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<TModel?> GetByIdAsync(string id);

    Task<bool> CheckByUsernameAsync(string username);

    Task<AppUserDto?> FindByIdentityUserIdAsync(string identityUserId);

    Task<IEnumerable<TModel>> FindByUsernameStartAtAsync(string startAt);
}
