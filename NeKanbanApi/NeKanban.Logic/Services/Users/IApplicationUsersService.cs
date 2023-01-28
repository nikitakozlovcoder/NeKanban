using NeKanban.Data.Entities;
using NeKanban.Logic.Models.UserModel;
using NeKanban.Logic.Services.ViewModels;

namespace NeKanban.Logic.Services.Users;

public interface IApplicationUsersService
{
    Task<ApplicationUserVm> Login(UserLoginModel userLoginModel, CancellationToken ct);
    Task<ApplicationUserVm> Register(UserRegisterModel userRegister, CancellationToken ct);
    Task<ApplicationUserVm> GetById(int id, CancellationToken ct);
    Task<ApplicationUser> Create(UserRegisterModel userRegisterModel, CancellationToken ct);
}