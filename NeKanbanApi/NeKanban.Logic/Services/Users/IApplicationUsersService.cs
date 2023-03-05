using NeKanban.Common.Entities;
using NeKanban.Common.Models.UserModel;
using NeKanban.Common.ViewModels;

namespace NeKanban.Logic.Services.Users;

public interface IApplicationUsersService
{
    Task<ApplicationUserWithTokenVm> Login(UserLoginModel userLoginModel, CancellationToken ct);
    Task Logout(UserRefreshTokenModel refreshTokenModel, CancellationToken ct);
    Task<ApplicationUserWithTokenVm> Register(UserRegisterModel userRegister, CancellationToken ct);
    Task<JwtTokenPair> RefreshToken(UserRefreshTokenModel refreshTokenModel, CancellationToken ct);
    Task<ApplicationUserWithTokenVm> GetById(int id, CancellationToken ct);
    Task<ApplicationUser> Create(UserRegisterModel userRegisterModel, CancellationToken ct);
}