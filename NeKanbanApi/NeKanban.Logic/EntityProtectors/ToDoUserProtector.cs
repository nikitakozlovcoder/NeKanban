﻿using Batteries.Injection.Attributes;
using Batteries.Repository;
using JetBrains.Annotations;
using NeKanban.Common.Entities;
using NeKanban.Logic.Services.Security;

namespace NeKanban.Logic.EntityProtectors;

[UsedImplicitly]
[Injectable<IEntityProtector<ToDoUser>>]
public class ToDoUserProtector : BaseEntityProtector<ToDoUser>
{
    private readonly IRepository<ToDoUser> _toDoUserRepository;
    public ToDoUserProtector(IPermissionCheckerService permissionCheckerService,
        IRepository<ToDoUser> toDoUserRepository) : base(permissionCheckerService)
    {
        _toDoUserRepository = toDoUserRepository;
    }

    protected override async Task<int?> GetDeskId(int entityId, CancellationToken ct)
    {
        var deskId = await _toDoUserRepository.First(x => x.Id == entityId, 
            x => x.DeskUser == null ? null : (int?)x.DeskUser.DeskId, ct: ct);
        return deskId;
    }
}