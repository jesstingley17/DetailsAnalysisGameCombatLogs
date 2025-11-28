using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.IdentityDAL.Services;

internal class ContextService(AppIdentityContext context) : IContextService
{
    private readonly AppIdentityContext _context = context;
    private IDbContextTransaction _transaction;

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
