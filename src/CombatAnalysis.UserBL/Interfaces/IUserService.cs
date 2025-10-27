using CombatAnalysis.UserBL.DTO;

namespace CombatAnalysis.UserBL.Interfaces;

public interface IUserService<TModel>
    where TModel : class
{
    Task<TModel?> CreateAsync(TModel item);

    Task UpdateAsync(string id, TModel item);

    Task DeleteAsync(string id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<TModel?> GetByIdAsync(string id);

    Task<bool> CheckByUsernameAsync(string username);

    Task<AppUserDto?> FindByIdentityUserIdAsync(string identityUserId);

    Task<IEnumerable<TModel>> FindByUsernameStartAtAsync(string startAt);
}
