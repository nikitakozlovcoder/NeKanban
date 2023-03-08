using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.ApplicationUsers;

namespace NeKanban.Common.ViewModels.ApplicationUsers;

public class ApplicationUserVm : BaseEntityModel<int>, IMapFrom<ApplicationUserDto, ApplicationUserVm>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
}