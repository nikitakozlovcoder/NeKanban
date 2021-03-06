using System.Net;
using Microsoft.AspNetCore.Identity;
using NeKanban.Data.Entities;
using NeKanban.ExceptionHandling;

namespace NeKanban.Services;

public abstract class BaseService
{
    protected readonly UserManager<ApplicationUser> UserManager;
    protected BaseService(UserManager<ApplicationUser> userManager)
    {
        UserManager = userManager;
    }
    
    protected static void EnsureEntityExists<TEntity>(TEntity entity)
    {
        if (entity == null)
        {
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"{typeof(TEntity).Name} not found");
        }   
    }

}