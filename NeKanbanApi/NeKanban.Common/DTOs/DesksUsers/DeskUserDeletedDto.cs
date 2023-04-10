using AutoMapper;
using Batteries.Mapper.Interfaces;
using NeKanban.Common.DTOs.ApplicationUsers;
using NeKanban.Common.Entities;

namespace NeKanban.Common.DTOs.DesksUsers;

public class DeskUserDeletedDto: BaseEntityModel<int>, IAutoMapFrom<DeskUser, DeskUserDeletedDto>
{
    public required int UserId { get; set; }
    public required ApplicationUserDto User { get; set; }
    public required DeskUserDeletionReason? DeletionReason { get; set; }
    
    public static void ConfigureMap(IMappingExpression<DeskUser, DeskUserDeletedDto> cfg)
    {
    }
}