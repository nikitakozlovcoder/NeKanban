namespace Batteries.Repository;

public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IHasPk<int>
{
}