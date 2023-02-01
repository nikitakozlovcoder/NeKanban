﻿using System.Linq.Expressions;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Data.Infrastructure;

public interface IRepository<TEntity> where TEntity: class, IHasPk<int>, new()
{
    Task Create(TEntity item, CancellationToken ct);
    Task Remove(TEntity item, CancellationToken ct);
    Task Update(TEntity item, CancellationToken ct);
    
    Task<List<TEntity>> ToList(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<List<T>> ToList<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, IEnumerable<Expression<Func<TEntity, object>>>? orders = null, CancellationToken ct = default);
    
    Task<TEntity> First(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity?> SingleOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<T> First<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, CancellationToken ct);
    Task<T?> FirstOrDefault<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, CancellationToken ct);
    Task<T> Single<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, CancellationToken ct);
    Task<T?> SingleOrDefault<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, CancellationToken ct);
    
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
    
    IQueryable<TEntity> QueryableSelect();
}