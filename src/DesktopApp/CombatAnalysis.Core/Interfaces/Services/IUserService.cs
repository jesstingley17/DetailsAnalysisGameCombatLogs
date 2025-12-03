using CombatAnalysis.Core.Models.User;

namespace CombatAnalysis.Core.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<AppUserModel>> LoadUsersAsync();
}
