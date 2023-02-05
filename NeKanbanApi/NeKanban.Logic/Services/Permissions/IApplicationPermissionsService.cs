using NeKanban.Common.ViewModels;

namespace NeKanban.Logic.Services.Permissions;

public interface IApplicationPermissionsService
{
    public List<ApplicationPermissionVm> GetApplicationPermissions();
}