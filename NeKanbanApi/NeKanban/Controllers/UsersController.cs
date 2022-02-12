using Microsoft.AspNetCore.Mvc;
using NeKanban.Controllers.Models;
using NeKanban.Services.Users;
using NeKanban.Services.ViewModels;

namespace NeKanban.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UsersController : ControllerBase
{
    private readonly IApplicationUsersService _applicationUsersService;
    
    public UsersController(IApplicationUsersService applicationUsersService)
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