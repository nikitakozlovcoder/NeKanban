using Batteries.Mapper.AppMapper;
using Microsoft.EntityFrameworkCore;

namespace Batteries.Repository;

public class Repository<TEntity> : Repository<TEntity, int>, IRepository<TEntity> where TEntity : class, IHasPk<int>
{
    public Repository(DbContext context, IAppMapper mapper) : base(context, mapper)
    {
    }
}