using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class CombatTransactionService(IContextService context) : ICombatTransactionService
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
