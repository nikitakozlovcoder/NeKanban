using Batteries.Injection.Attributes;
using Batteries.Mapper.AppMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Batteries.Repository;

[UsedImplicitly]
[Injectable(typeof(IRepository<>))]
public class Repository<TEntity> : Repository<TEntity, int>, IRepository<TEntity> where TEntity : class, IHasPk<int>
{
    public Repository(DbContext context, IAppMapper mapper) : base(context, mapper)
    {
    }
}