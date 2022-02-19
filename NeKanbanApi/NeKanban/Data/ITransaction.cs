using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace NeKanban.Data;

public interface ITransactionFactory
{
    Task<IDbContextTransaction> CreateTransaction();
}