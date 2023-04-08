using Batteries.Mapper.AppMapper;
using Microsoft.EntityFrameworkCore;

namespace Batteries.Repository;

public class GuidRepository<TEntity> : Repository<TEntity, Guid>, IGuidRepository<TEntity> where TEntity : class, IHasPk<Guid>
{
    public GuidRepository(DbContext context, IAppMapper mapper) : base(context, mapper)
    {
    }
}