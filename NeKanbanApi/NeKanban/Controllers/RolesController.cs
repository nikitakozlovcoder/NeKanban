using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Constants.Security;
using NeKanban.Data.Entities;
using NeKanban.Services.ViewModels;

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