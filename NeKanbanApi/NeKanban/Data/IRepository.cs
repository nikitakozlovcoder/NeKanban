using System.Linq.Expressions;

namespace NeKanban.Data;

public interface IRepository<TEntity> where TEntity: class, IHasPk<int>
{
    Task Create(TEntity item, CancellationToken ct);
    Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity?> GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    IQueryable<TEntity> QueryableSelect();
    Task Remove(TEntity item, CancellationToken ct);
    Task Update(TEntity item, CancellationToken ct);
}