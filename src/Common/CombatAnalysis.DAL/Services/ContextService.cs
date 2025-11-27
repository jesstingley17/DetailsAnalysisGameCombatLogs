using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.DAL.Services;

internal class ContextService(CombatParserContext context) : IContextService
{
    private readonly CombatParserContext _context = context;
    private IDbContextTransaction? _transaction;

    public async Task BeginAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
