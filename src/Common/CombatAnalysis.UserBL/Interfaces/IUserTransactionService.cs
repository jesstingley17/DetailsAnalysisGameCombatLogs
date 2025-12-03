namespace CombatAnalysis.UserBL.Interfaces;

/// <summary>
/// Contract for User SQL DB transaction
/// </summary>
public interface IUserTransactionService
{
    /// <summary>
    /// Start async SQL DB transaction
    /// </summary>
    /// <returns>Task as result of async operation</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// Execute async SQL DB transaction and release all resources
    /// </summary>
    /// <returns>Task as result of async operation</returns>
    Task CommitTransactionAsync();

    /// <summary>
    /// Execute async SQL DB rollback transaction and release all resources
    /// </summary>
    /// <returns>Task as result of async operation</returns>
    Task RollbackTransactionAsync();
}
