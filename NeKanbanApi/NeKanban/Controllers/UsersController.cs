using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Controllers.Models.UserModel;
using NeKanban.Data.Entities;
using NeKanban.Services.Users;
using NeKanban.Services.ViewModels;

namespace NeKanban.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]/[action]")]
public class UsersController : BaseAuthController
{
    private readonly IApplicationUsersService _applicationUsersService;
    
    public UsersController(IApplicationUsersService applicationUsersService,
        UserManager<ApplicationUser> userManager,
        IServiceProvider serviceProvider) : base(userManager, serviceProvider)
    {
        _applicationUsersService = applicationUsersService;
    }
    
    [HttpPost]
    public  Task<ApplicationUserVm> LogIn([FromBody]UserLoginModel userLoginModel, CancellationToken ct = default)
    {
        return _applicationUsersService.Login(userLoginModel, ct);
    }
    
    [HttpPost]
    public Task<ApplicationUserVm> Register([FromBody]UserRegisterModel userRegisterModel, CancellationToken ct = default)
    {
        return _applicationUsersService.Register(userRegisterModel, ct);
    }
    
}