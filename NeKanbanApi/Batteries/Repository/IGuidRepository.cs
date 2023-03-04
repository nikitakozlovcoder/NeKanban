namespace Batteries.Repository;

public interface IGuidRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : class, IHasPk<Guid>
{
    
}