using Microsoft.AspNetCore.Mvc;
using NeKanban.Common.ViewModels;
using NeKanban.Logic.Services.Permissions;

namespace NeKanban.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ApplicationPermissionsController : Controller
{
    private readonly IApplicationPermissionsService _applicationPermissionsService;

    public ApplicationPermissionsController(IApplicationPermissionsService applicationPermissionsService)
    {
        _applicationPermissionsService = applicationPermissionsService;
    }

    [HttpGet]
    public List<ApplicationPermissionVm> GetPermissions()
    {
        return _applicationPermissionsService.GetApplicationPermissions();
    }
}
