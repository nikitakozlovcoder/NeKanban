using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using IsolationLevel = System.Data.IsolationLevel;

namespace NeKanban.Data;

public class TransactionFactory : ITransactionFactory
{
    private readonly ApplicationContext _context;

    public TransactionFactory(ApplicationContext context)
    {
        _context = context;
    }

    public Task<IDbContextTransaction> CreateTransaction()
    {
        return _context.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
    }
}