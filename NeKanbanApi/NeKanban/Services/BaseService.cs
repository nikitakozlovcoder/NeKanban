using Microsoft.AspNetCore.Identity;
using NeKanban.Data.Entities;

namespace NeKanban.Services;

public abstract class BaseService
{
    protected readonly UserManager<ApplicationUser> UserManager;
    protected BaseService(UserManager<ApplicationUser> userManager)
    {
        UserManager = userManager;
    }
    
}