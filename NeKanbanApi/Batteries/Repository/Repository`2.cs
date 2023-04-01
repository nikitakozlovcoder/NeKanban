﻿using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Batteries.Exceptions;
using Batteries.Mapper.AppMapper;
using Batteries.Mapper.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Batteries.Repository;

public abstract class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
    where TEntity : class, IHasPk<TPrimaryKey>
    where TPrimaryKey : IComparable<TPrimaryKey>
{
    private readonly DbContext _context;
    protected readonly DbSet<TEntity> EntityDbSet;
    private readonly IAppMapper _mapper;

    public Repository(DbContext context, IAppMapper mapper)
    {
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        EntityDbSet = context.Set<TEntity>();
        _context = context;
        _mapper = mapper;
    }

    #region write
    public Task Create(TEntity item, CancellationToken ct)
    {
        _context.Entry(item).State = EntityState.Added;
        return _context.SaveChangesAsync(ct);
    }

    public Task Create(IEnumerable<TEntity> items, CancellationToken ct)
    {
        foreach (var item in items)
        {
            _context.Entry(item).State = EntityState.Added;
        }
        
        return _context.SaveChangesAsync(ct);
    }

    public Task CreateRecursive(TEntity entity, CancellationToken ct)
    {
        EntityDbSet.Add(entity);
        return _context.SaveChangesAsync(ct);
    }

    public Task Remove(TEntity item, CancellationToken ct)
    {
        EntityDbSet.Remove(item);
        return _context.SaveChangesAsync(ct);
    }

    public Task Remove(IEnumerable<TEntity> items, CancellationToken ct)
    {
        EntityDbSet.RemoveRange(items);
        return _context.SaveChangesAsync(ct);
    }

    public async Task<TEntity> Remove(TPrimaryKey id, CancellationToken ct)
    {
        var item = await Single(x => x.Id.Equals(id), ct);
        await Remove(item, ct);
        return item;
    }

    public async Task<List<TEntity>> Remove(IEnumerable<TPrimaryKey> ids, CancellationToken ct)
    {
        var items = await ToList(x => ids.Contains(x.Id), ct);
        await Remove(items, ct);
        return items;
    }

    public async Task Revoke<T>(T entity, CancellationToken ct) where T : TEntity, ISoftDeletable
    {
        entity.IsDeleted = false;
        await Update(entity, ct);
    }

    public async Task<TEntity> Remove(int id, CancellationToken ct)
    {
        var item = await Single(x => x.Id.Equals(id), ct);
        await Remove(item, ct);
        return item;
    }

    public Task Update(TEntity item, CancellationToken ct)
    {
        _context.Entry(item).State = EntityState.Modified;
        return _context.SaveChangesAsync(ct);
    }

    public Task Update(IEnumerable<TEntity> items, CancellationToken ct)
    {
        foreach (var item in items)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        return _context.SaveChangesAsync(ct);
    }

    #endregion

    #region ToList
    public Task<List<TEntity>> ToList(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return EntityDbSet.Where(predicate).ToListAsync(ct);
    }

    public Task<List<T>> ToList<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, IEnumerable<Expression<Func<TEntity, object>>>? orders = null, CancellationToken ct = default)
    {
        return Query(predicate, orders).Select(projection).ToListAsync(ct);
    }
    #endregion
    
    #region First/Single
    public async Task<TEntity> First(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return ThrowOnNull(await EntityDbSet.Where(predicate).FirstOrDefaultAsync(ct));
    }
    
    public Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return EntityDbSet.Where(predicate).FirstOrDefaultAsync(ct);
    }

    public async Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return ThrowOnNull(await EntityDbSet.Where(predicate).SingleOrDefaultAsync(ct));
    }

    public Task<TEntity?> SingleOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return EntityDbSet.Where(predicate).SingleOrDefaultAsync(ct);
    }

    public async Task<T> First<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, CancellationToken ct)
    {
        return ThrowOnNull(await FirstOrDefault(predicate, projection, ct));
    }

    public Task<T?> FirstOrDefault<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, CancellationToken ct)
    {
        return Query(predicate, null).Select(projection).FirstOrDefaultAsync(ct);
    }

    public async Task<T> Single<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, CancellationToken ct)
    {
        return ThrowOnNull(await SingleOrDefault(predicate, projection, ct));
    }

    public Task<T?> SingleOrDefault<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> projection, CancellationToken ct)
    {
        return Query(predicate, null).Select(projection).SingleOrDefaultAsync(ct);
    }
    #endregion

    #region ProjectTo
    public Task<List<T>> ProjectTo<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct) where T : class, IAutoMapFrom<TEntity, T>
    {
        return Query(predicate, null).ProjectTo<T>(_mapper.ConfigurationProvider).ToListAsync(ct);
    }
    
    public Task<List<T>> ProjectTo<T>(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct) where T : class, IAutoMapFrom<TEntity, T>
    {
        return Query(predicate, orders).ProjectTo<T>(_mapper.ConfigurationProvider).ToListAsync(ct);
    }
    
    public async Task<T> ProjectToSingle<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct) where T : class, IAutoMapFrom<TEntity, T>
    {
        return ThrowOnNull(await Query(predicate, null).ProjectTo<T>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(ct));
    }
    
    public async Task<T> ProjectToSingle<T>(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct) where T : class, IAutoMapFrom<TEntity, T>
    {
        return ThrowOnNull(await Query(predicate, orders).ProjectTo<T>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(ct));
    }
    
    public Task<T?> ProjectToFirstOrDefault<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct) where T : class, IAutoMapFrom<TEntity, T>
    {
        return Query(predicate, null).ProjectTo<T>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(ct);
    }
    
    public Task<T?> ProjectToFirstOrDefault<T>(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct) where T : class, IAutoMapFrom<TEntity, T>
    {
        return Query(predicate, orders).ProjectTo<T>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(ct);
    }
    
    public async Task<T> ProjectToFirst<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct) where T : class, IAutoMapFrom<TEntity, T>
    {
        return ThrowOnNull(await ProjectToFirstOrDefault<T>(predicate, ct));
    }
    
    public async Task<T> ProjectToFirst<T>(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct) where T : class, IAutoMapFrom<TEntity, T>
    {
        return ThrowOnNull(await ProjectToFirstOrDefault<T>(predicate, orders, ct));
    }
    #endregion

    #region Any
    public async Task AnyOrThrow(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        var exists = await Any(predicate, ct);
        if (!exists)
        {
            throw new EntityDoesNotExists<TEntity>();
        }
    }

    public Task<bool> Any(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return EntityDbSet.AnyAsync(predicate, ct);
    }
    #endregion
    
    public IQueryable<TEntity> QueryableSelect()
    {
        return EntityDbSet;
    }
    
    private IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>>? orders)
    {
        var query = EntityDbSet.Where(predicate);
        if (orders != null)
        {
            query = orders.Aggregate(query.OrderBy(x => true), (current, order) => current.ThenBy(order));
        }
        
        return query;
    }
    
    private static T ThrowOnNull<T>(T? entity)
    {
        if (entity == null)
        {
            throw new EntityDoesNotExists<TEntity>();
        }

        return entity;
    }
}