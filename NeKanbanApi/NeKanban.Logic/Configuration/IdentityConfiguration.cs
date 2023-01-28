using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.Configuration;

public static class IdentityConfiguration
{
    public static void AddAppIdentity(this IServiceCollection serviceCollection)
    {
       serviceCollection.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddSignInManager()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
    }
}