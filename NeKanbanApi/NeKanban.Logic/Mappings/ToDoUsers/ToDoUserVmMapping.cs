using Batteries.Injection.Attributes;
using Batteries.Mapper;
using Batteries.Mapper.AppMapper;
using Batteries.Mapper.Interfaces;
using JetBrains.Annotations;
using NeKanban.Common.DTOs.DesksUsers;
using NeKanban.Common.DTOs.ToDoUsers;
using NeKanban.Common.ViewModels.DesksUsers;
using NeKanban.Common.ViewModels.ToDoUsers;

namespace NeKanban.Logic.Mappings.ToDoUsers;

[UsedImplicitly]
[Injectable<IMappingProfile<ToDoUserDto, ToDoUserVm>>]
public class ToDoUserVmMapping : BaseMappingProfile<ToDoUserDto, ToDoUserVm>
{
    private readonly IAppMapper _mapper;

    public ToDoUserVmMapping(IAppMapper mapper)
    {
        _mapper = mapper;
    }
    
    public override async Task<ToDoUserVm> Map(ToDoUserDto source, CancellationToken ct)
    {
        var user = source.DeskUser == null ? null : await _mapper.Map<DeskUserLiteVm, DeskUserLiteDto>(source.DeskUser, ct);
        return new ToDoUserVm
        {
            Id = source.Id,
            DeskUser = user,
            ToDoUserType = source.ToDoUserType
        };
    }
}