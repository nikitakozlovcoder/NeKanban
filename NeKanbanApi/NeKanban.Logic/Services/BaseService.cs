using System.Net;
using Batteries.Exceptions;

namespace NeKanban.Logic.Services;

public abstract class BaseService
{
    protected static void EnsureEntityExists<TEntity>(TEntity entity)
    {
        if (entity == null)
        {
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"{typeof(TEntity).Name} not found");
        }   
    }
}