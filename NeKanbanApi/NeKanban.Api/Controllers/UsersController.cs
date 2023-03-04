using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.UserModel;
using NeKanban.Common.ViewModels;
using NeKanban.Controllers.Auth;
using NeKanban.Logic.Services.Users;

namespace NeKanban.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]/[action]")]
public class UsersController : BaseAuthController
{
    private readonly IApplicationUsersService _applicationUsersService;
    
    public UsersController(
        IApplicationUsersService applicationUsersService,
        UserManager<ApplicationUser> userManager,
        IServiceProvider serviceProvider) : base(userManager, serviceProvider)
    {
        _applicationUsersService = applicationUsersService;
    }
    
    [HttpPost]
    public Task<ApplicationUserWithTokenVm> LogIn([FromBody]UserLoginModel userLoginModel, CancellationToken ct = default)
    {
        return _applicationUsersService.Login(userLoginModel, ct);
    }
    
    [HttpPost]
    public Task<ApplicationUserWithTokenVm> Register([FromBody]UserRegisterModel userRegisterModel, CancellationToken ct = default)
    {
        return _applicationUsersService.Register(userRegisterModel, ct);
    }
    
    [HttpPost]
    public Task<JwtTokenPair> Refresh([FromBody]UserRefreshTokenModel userRefreshTokenModel, CancellationToken ct = default)
    {
        return _applicationUsersService.RefreshToken(userRefreshTokenModel, ct);
    }
}
