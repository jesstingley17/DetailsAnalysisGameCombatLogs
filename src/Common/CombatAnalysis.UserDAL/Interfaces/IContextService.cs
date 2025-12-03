namespace CombatAnalysis.UserDAL.Interfaces;

/// <summary>
/// Contract for SQL DB transaction
/// </summary>
public interface IContextService
{
    /// <summary>
    /// Start async SQL DB transaction
    /// </summary>
    /// <returns>Task as result of async operation</returns>
    Task BeginAsync();

    /// <summary>
    /// Execute async SQL DB transaction and release all resources
    /// </summary>
    /// <returns>Task as result of async operation</returns>
    Task CommitAsync();

    /// <summary>
    /// Execute async SQL DB rollback transaction and release all resources
    /// </summary>
    /// <returns>Task as result of async operation</returns>
    Task RollbackAsync();
}
