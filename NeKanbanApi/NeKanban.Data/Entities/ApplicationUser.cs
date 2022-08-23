using Microsoft.AspNetCore.Identity;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Data.Entities;

public class ApplicationUser : IdentityUser<int>, IHasPk<int>
{
    public ApplicationUser()
    {
        base.UserName = Guid.NewGuid().ToString();
    }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}