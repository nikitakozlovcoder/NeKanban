using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Data.Entities;

namespace NeKanban.Controllers;

public class BaseAuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    public BaseAuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    protected Task<ApplicationUser> GetApplicationUser()
    {
        return _userManager.GetUserAsync(User);
    }
  
}