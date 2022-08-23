using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Controllers.Auth;
using NeKanban.Data.Entities;
using NeKanban.Logic.SecurityProfile;
using NeKanban.Logic.Services.ViewModels;
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
