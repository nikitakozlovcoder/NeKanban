using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Constants.Security;
using NeKanban.Data.Entities;
using NeKanban.ExceptionHandling;
using NeKanban.Security;

namespace NeKanban.Controllers;

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
        return _userManager.GetUserAsync(User);
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

}