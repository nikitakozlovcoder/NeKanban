using Batteries.Injection.Attributes;
using Batteries.Mapper.AppMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Batteries.Repository;

[UsedImplicitly]
[Injectable(typeof(IGuidRepository<>))]
public class GuidRepository<TEntity> : Repository<TEntity, Guid>, IGuidRepository<TEntity> where TEntity : class, IHasPk<Guid>
{
    public GuidRepository(DbContext context, IAppMapper mapper) : base(context, mapper)
    {
    }
}