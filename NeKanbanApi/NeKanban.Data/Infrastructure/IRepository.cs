using System.Linq.Expressions;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Data.Infrastructure;

public interface IRepository<TEntity> where TEntity: class, IHasPk<int>, new()
{
    Task Create(TEntity item, CancellationToken ct);
    Task<List<TEntity>> ToList(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity> First(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity?> SingleOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    IQueryable<TEntity> QueryableSelect();
    Task Remove(TEntity item, CancellationToken ct);
    Task Update(TEntity item, CancellationToken ct);
    Task<List<T>> ProjectTo<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct) where T : IMapFrom<TEntity, T>, new();
    Task<List<T>> ProjectTo<T>(Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct)
        where T : IMapFrom<TEntity, T>, new();
    Task<T> ProjectToSingle<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        where T : IMapFrom<TEntity, T>, new();
    Task<T> ProjectToSingle<T>(Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct)
        where T : IMapFrom<TEntity, T>, new();
    Task<T?> ProjectToFirstOrDefault<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        where T : IMapFrom<TEntity, T>, new();
    Task<T?> ProjectToFirstOrDefault<T>(Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct)
        where T : IMapFrom<TEntity, T>, new();
    Task<T> ProjectToFirst<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        where T : IMapFrom<TEntity, T>, new();
    Task<T> ProjectToFirst<T>(Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct)
        where T : IMapFrom<TEntity, T>, new();
    Task AnyOrThrow(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
}