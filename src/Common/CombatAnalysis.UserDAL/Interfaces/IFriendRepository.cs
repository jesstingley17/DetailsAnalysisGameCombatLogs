using CombatAnalysis.UserDAL.DTO;
using CombatAnalysis.UserDAL.Entities;

namespace CombatAnalysis.UserDAL.Interfaces;

/// <summary>
/// Contract for Friend DB entity
/// </summary>
public interface IFriendRepository
{
    /// <summary>
    /// Create a new entity in DB
    /// </summary>
    /// <param name="item">Item that reflect all properties of new entity. Id should not be set</param>
    /// <returns>Return created entity. Entity could be null if entity with provided Id already exist</returns>
    Task<FriendDto?> CreateAsync(Friend item);


    /// <summary>
    /// Delete exlreadt exist entity by entity ID
    /// </summary>
    /// <param name="id">Entity Id</param>
    /// <returns>Return count of affected rows in DB</returns>
    Task<int> DeleteAsync(int id);

    /// <summary>
    /// Get entity by Id
    /// </summary>
    /// <param name="id">Entity Id</param>
    /// <returns>Return found entity. Entity could be null if entity with provided Id not exist</returns>
    Task<FriendDto?> GetByIdAsync(int id);

    /// <summary>
    /// Get collection of entity by provided property (property name + values)
    /// </summary>
    /// <param name="paramName">Property name, that reflect one of entity property</param>
    /// <param name="value">Property value</param>
    /// <returns>Return found collection of entity. Collection be empty if no one be found</returns>
    Task<IEnumerable<FriendDto>> GetByParamAsync(string paramName, object value);

    /// <summary>
    /// Get collection of all entity
    /// </summary>
    /// <returns>Return found collection of entity. Collection be empty if no one be found</returns>
    Task<IEnumerable<FriendDto>> GetAllAsync();
}
