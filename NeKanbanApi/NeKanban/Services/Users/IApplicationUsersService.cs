using NeKanban.Controllers.Models;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.Users;

public interface IApplicationUsersService
{
    Task<ApplicationUserVm> Login<T>(T userLoginModel, CancellationToken ct) where T : UserLoginModel;
    Task<ApplicationUserVm> Register(UserRegisterModel userRegister, CancellationToken ct);
    Task<ApplicationUserVm> GetById(int id, CancellationToken ct);
}