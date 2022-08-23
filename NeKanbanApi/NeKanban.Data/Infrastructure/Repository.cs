using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeKanban.Data.Entities;

namespace NeKanban.Data.Infrastructure;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IHasPk<int>
{
    private readonly IdentityDbContext<ApplicationUser, ApplicationRole, int> _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(IdentityDbContext<ApplicationUser, ApplicationRole, int> context)
    {
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        _dbSet = context.Set<TEntity>();
        _context = context;
    }

    public Task Create(TEntity item, CancellationToken ct)
    {
        _context.Entry(item).State = EntityState.Added;
        return _context.SaveChangesAsync(ct);
    }

    public Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return _dbSet.Where(predicate).ToListAsync(ct);
    }
    
    public Task<TEntity?> GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return _dbSet.Where(predicate).FirstOrDefaultAsync(ct);
    }

    public IQueryable<TEntity> QueryableSelect()
    {
        return _dbSet;
    }

    public Task Remove(TEntity item, CancellationToken ct)
    {
        _dbSet.Remove(item);
        return _context.SaveChangesAsync(ct);
    }

    public Task Update(TEntity item, CancellationToken ct)
    {
        _context.Entry(item).State = EntityState.Modified;
        return _context.SaveChangesAsync(ct);
    }
}