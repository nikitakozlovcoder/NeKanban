using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Common.Entities;
using NeKanban.Common.ViewModels;
using NeKanban.Controllers.Auth;
using NeKanban.Logic.SecurityProfile;
using NeKanban.Security.Constants;

namespace NeKanban.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class RolesController : BaseAuthController
{
    public RolesController(
        UserManager<ApplicationUser> userManager,
        IServiceProvider serviceProvider) : base(userManager, serviceProvider)
    {
    }
    [HttpGet]
    public List<DeskRoleVm> GetRolesAndPermissions()
    {
        var permissionRoleMapping = new PermissionsRoleMapping();
        return permissionRoleMapping.DeskRoles;
    }
}
