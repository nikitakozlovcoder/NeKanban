using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.Entities;
using NeKanban.Logic.EntityProtectors;
using NeKanban.Security.Constants;

namespace NeKanban.Controllers.Auth;

public class BaseAuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IServiceProvider _serviceProvider;
    public BaseAuthController(UserManager<ApplicationUser> userManager, IServiceProvider serviceProvider)
    {
        _userManager = userManager;
        _serviceProvider = serviceProvider;
    }

    protected Task<ApplicationUser> GetApplicationUser()
    {
        return _userManager.GetUserAsync(User)!;
    }

    protected async Task EnsureAbleTo<TProtectedEntity>(PermissionType permissionType, int entityId, CancellationToken ct)
    {
        var protector = _serviceProvider.GetRequiredService<IEntityProtector<TProtectedEntity>>();
        var user = await GetApplicationUser();
        if (!await protector.HasPermission(user, permissionType, entityId, ct))
        {
            throw new HttpStatusCodeException(HttpStatusCode.Forbidden);   
        }
    }
    
    protected async Task EnsureAbleTo<TProtectedEntity>(int entityId, CancellationToken ct)
    {
        var protector = _serviceProvider.GetRequiredService<IEntityProtector<TProtectedEntity>>();
        var user = await GetApplicationUser();
        if (!await protector.HasPermission(user, entityId, ct))
        {
            throw new HttpStatusCodeException(HttpStatusCode.Forbidden);   
        }
    }
}
