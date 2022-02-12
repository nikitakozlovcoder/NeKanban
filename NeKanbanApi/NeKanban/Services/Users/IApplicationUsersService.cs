using NeKanban.Controllers.Models;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.Users;

public interface IApplicationUsersService
{
    Task<ApplicationUserVm> Login(UserLoginModel userLoginModel, CancellationToken ct);
    Task<ApplicationUserVm> Register(UserRegisterModel userRegister, CancellationToken ct);
    Task<ApplicationUserVm> GetById(int id, CancellationToken ct);
}