using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.Entities;

namespace NeKanban.Common.DTOs.ApplicationUsers;

public class ApplicationUserDto : BaseEntityModel<int>, IAutoMapFrom<ApplicationUser, ApplicationUserDto>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public static void ConfigureMap(IMappingExpression<ApplicationUser, ApplicationUserDto> cfg)
    {
    }
}