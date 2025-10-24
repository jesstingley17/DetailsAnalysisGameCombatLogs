using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserDAL.Interfaces;

namespace CombatAnalysis.UserBL.Services;

internal class CustomerTransactionService(IContextService context) : ICustomerTransactionService
{
    private readonly IContextService _context = context;

    public async Task BeginTransactionAsync()
    {
        await _context.BeginAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.RollbackAsync();
    }
}
