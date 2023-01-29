using System.Linq.Expressions;
using NeKanban.Common.Entities;

namespace NeKanban.Data.Infrastructure;

public interface IRepository<TEntity> where TEntity: class, IHasPk<int>
{
    Task Create(TEntity item, CancellationToken ct);
    Task<List<TEntity>> ToList(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    IQueryable<TEntity> QueryableSelect();
    Task Remove(TEntity item, CancellationToken ct);
    Task Update(TEntity item, CancellationToken ct);
}