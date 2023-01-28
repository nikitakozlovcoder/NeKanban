using NeKanban.Common.Attributes;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Models.DeskModels;
using NeKanban.Logic.Models.UserModel;
using NeKanban.Logic.Services.Desks;
using NeKanban.Logic.Services.Users;

namespace NeKanban.Logic.Seeders;

[Injectable<ISeeder>]
public class InitialSeeder : ISeeder
{
    private readonly IApplicationUsersService _applicationUsersService;
    private readonly IDesksService _desksService;

    public InitialSeeder(IApplicationUsersService applicationUsersService,
        IDesksService desksService)
    {
        _applicationUsersService = applicationUsersService;
        _desksService = desksService;
    }

    public async Task Run(CancellationToken ct)
    {
        var user = await _applicationUsersService.Create(new UserRegisterModel
        {
            Name = "Test",
            Surname = "Test",
            Email = "test@test.test",
            Password = "password",
        }, ct);
        
        await _desksService.CreateDesk(new DeskCreateModel
        {
            Name = "Test desk"
        }, user, ct);
    }
}