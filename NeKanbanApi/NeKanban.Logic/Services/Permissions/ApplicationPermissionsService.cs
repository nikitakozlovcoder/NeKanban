using Batteries.Injection.Attributes;
using JetBrains.Annotations;
using NeKanban.Common.ViewModels;
using NeKanban.Security.Constants;

namespace NeKanban.Logic.Services.Permissions;

[UsedImplicitly]
[Injectable<IApplicationPermissionsService>]
public class ApplicationPermissionsService : IApplicationPermissionsService
{
    public List<ApplicationPermissionVm> GetApplicationPermissions()
    {
        var permissions = Enum.GetValues<PermissionType>()
            .Select(x => new ApplicationPermissionVm
        {
            Permission = x
        });

        return permissions.ToList();
    }
}