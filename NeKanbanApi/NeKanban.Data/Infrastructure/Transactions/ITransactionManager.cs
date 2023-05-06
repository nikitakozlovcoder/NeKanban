using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace NeKanban.Data.Infrastructure.Transactions;

public interface ITransactionManager
{
    Task<IDbContextTransaction> CreateScope(CancellationToken ct);
    Task<IDbContextTransaction> CreateScope(IsolationLevel isolationLevel, CancellationToken ct);
}