using NeKanban.Common.Entities;
using NeKanban.Common.Models.UserModel;
using NeKanban.Common.ViewModels;

namespace NeKanban.Logic.Services.Users;

public interface IApplicationUsersService
{
    Task<ApplicationUserVm> Login(UserLoginModel userLoginModel, CancellationToken ct);
    Task<ApplicationUserVm> Register(UserRegisterModel userRegister, CancellationToken ct);
    Task<ApplicationUserVm> GetById(int id, CancellationToken ct);
    Task<ApplicationUser> Create(UserRegisterModel userRegisterModel, CancellationToken ct);
}