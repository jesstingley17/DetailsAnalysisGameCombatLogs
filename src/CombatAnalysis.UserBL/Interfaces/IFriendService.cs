using CombatAnalysis.UserBL.DTO;

namespace CombatAnalysis.UserBL.Interfaces;

public interface IFriendService
{
    Task<FriendDto> CreateAsync(FriendDto item);

    Task<int> UpdateAsync(FriendDto item);

    Task<int> DeleteAsync(int id);

    Task<IEnumerable<FriendDto>> GetAllAsync();

    Task<IEnumerable<FriendDto>> GetByParamAsync(string paramName, object value);

    Task<FriendDto> GetByIdAsync(int id);
}
