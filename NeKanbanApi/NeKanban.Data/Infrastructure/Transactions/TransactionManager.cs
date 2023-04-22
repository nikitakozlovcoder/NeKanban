using System.Data;
using Batteries.Injection.Attributes;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace NeKanban.Data.Infrastructure.Transactions;

[UsedImplicitly]
[Injectable<ITransactionManager>]
public class TransactionManager : ITransactionManager
{
    private readonly ApplicationContext _applicationContext;

    public TransactionManager(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IDbContextTransaction> CreateScope(CancellationToken ct)
    {
        return await _applicationContext.Database.BeginTransactionAsync(ct);
    }
    
    public async Task<IDbContextTransaction> CreateScope(IsolationLevel isolationLevel, CancellationToken ct)
    {
        return await _applicationContext.Database.BeginTransactionAsync(isolationLevel, ct);
    }
}