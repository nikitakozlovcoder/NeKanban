using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.AppMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Exceptions;
using NeKanban.Common.Interfaces;

namespace NeKanban.Data.Infrastructure;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IHasPk<int>, new()
{
    private readonly IdentityDbContext<ApplicationUser, ApplicationRole, int> _context;
    protected readonly DbSet<TEntity> EntityDbSet;
    private readonly IAppMapper _mapper;

    public Repository(IdentityDbContext<ApplicationUser, ApplicationRole, int> context, IAppMapper mapper)
    {
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        EntityDbSet = context.Set<TEntity>();
        _context = context;
        _mapper = mapper;
    }

    public Task Create(TEntity item, CancellationToken ct)
    {
        _context.Entry(item).State = EntityState.Added;
        return _context.SaveChangesAsync(ct);
    }
    
    public Task Remove(TEntity item, CancellationToken ct)
    {
        EntityDbSet.Remove(item);
        return _context.SaveChangesAsync(ct);
    }

    public Task Update(TEntity item, CancellationToken ct)
    {
        _context.Entry(item).State = EntityState.Modified;
        return _context.SaveChangesAsync(ct);
    }

    public Task<List<TEntity>> ToList(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return EntityDbSet.Where(predicate).ToListAsync(ct);
    }
    
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

    public IQueryable<TEntity> QueryableSelect()
    {
        return EntityDbSet;
    }

    public Task<List<T>> ProjectTo<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct) where T : IMapFrom<TEntity, T>, new()
    {
        return ProjectToQuery<T>(predicate, null).ToListAsync(ct);
    }
    
    public Task<List<T>> ProjectTo<T>(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct) where T : IMapFrom<TEntity, T>, new()
    {
        return ProjectToQuery<T>(predicate, orders).ToListAsync(ct);
    }
    
    public async Task<T> ProjectToSingle<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct) where T : IMapFrom<TEntity, T>, new()
    {
        return ThrowOnNull(await ProjectToQuery<T>(predicate, null).SingleOrDefaultAsync(ct));
    }
    
    public async Task<T> ProjectToSingle<T>(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct) where T : IMapFrom<TEntity, T>, new()
    {
        return ThrowOnNull(await ProjectToQuery<T>(predicate, orders).SingleOrDefaultAsync(ct));
    }
    
    public Task<T?> ProjectToFirstOrDefault<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct) where T : IMapFrom<TEntity, T>, new()
    {
        return ProjectToQuery<T>(predicate, null).FirstOrDefaultAsync(ct);
    }
    
    public Task<T?> ProjectToFirstOrDefault<T>(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct) where T : IMapFrom<TEntity, T>, new()
    {
        return ProjectToQuery<T>(predicate, orders).FirstOrDefaultAsync(ct);
    }
    
    public async Task<T> ProjectToFirst<T>(Expression<Func<TEntity, bool>> predicate, CancellationToken ct) where T : IMapFrom<TEntity, T>, new()
    {
        return ThrowOnNull(await ProjectToQuery<T>(predicate, null).FirstOrDefaultAsync(ct));
    }
    
    public async Task<T> ProjectToFirst<T>(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> orders, CancellationToken ct) where T : IMapFrom<TEntity, T>, new()
    {
        return ThrowOnNull(await ProjectToQuery<T>(predicate, orders).FirstOrDefaultAsync(ct));
    }

    public async Task AnyOrThrow(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        var exists = await EntityDbSet.AnyAsync(predicate, ct);
        if (!exists)
        {
            throw new EntityDoesNotExists<TEntity>();
        }
    }

    private IQueryable<T> ProjectToQuery<T>(Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>>? orders)
        where T : IMapFrom<TEntity, T>, new()
    {
        var query = EntityDbSet.Where(predicate);
        if (orders != null)
        {
            query = orders.Aggregate(query.OrderBy(x => true), (current, order) => current.ThenBy(order));
        }
        
        return query.ProjectTo<T>(_mapper.ConfigurationProvider);
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