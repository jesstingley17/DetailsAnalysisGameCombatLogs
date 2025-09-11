using CombatAnalysis.UserDAL.DTO;
using CombatAnalysis.UserDAL.Entities;

namespace CombatAnalysis.UserDAL.Interfaces;

public interface IFriendRepository
{
    Task<FriendDto?> CreateAsync(Friend item);

    Task<int> UpdateAsync(Friend item);

    Task<int> DeleteAsync(int id);

    Task<FriendDto?> GetByIdAsync(int id);

    Task<IEnumerable<FriendDto>> GetByParamAsync(string paramName, object value);

    Task<IEnumerable<FriendDto>> GetAllAsync();
}
