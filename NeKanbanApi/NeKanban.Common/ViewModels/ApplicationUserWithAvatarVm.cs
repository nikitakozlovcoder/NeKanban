using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.ApplicationUsers;

namespace NeKanban.Common.ViewModels;

public class ApplicationUserWithAvatarVm :  BaseEntityModel<int>, IMapFrom<ApplicationUserDto, ApplicationUserWithAvatarVm>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string AvatarUrl { get; set; }
    public required string Email { get; set; }
}