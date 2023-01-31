using AutoMapper;
using NeKanban.Common.Entities;
using NeKanban.Common.Interfaces;

namespace NeKanban.Common.DTOs.ApplicationUsers;

public class ApplicationUserDto : BaseEntityDto<int>, IMapFrom<ApplicationUser, ApplicationUserDto>
{
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string Email { get; set; } = "";
    public static void ConfigureMap(IMappingExpression<ApplicationUser, ApplicationUserDto> cfg)
    {
    }
}