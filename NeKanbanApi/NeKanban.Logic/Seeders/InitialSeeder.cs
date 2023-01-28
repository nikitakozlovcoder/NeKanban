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
    private readonly IRepository<ApplicationUser> _userRepository;

    public InitialSeeder(IApplicationUsersService applicationUsersService,
        IDesksService desksService,
        IRepository<ApplicationUser> userRepository)
    {
        _applicationUsersService = applicationUsersService;
        _desksService = desksService;
        _userRepository = userRepository;
    }

    public async Task Run(CancellationToken ct)
    {
        var userVm = await _applicationUsersService.Create(new UserRegisterModel
        {
            Name = "Test",
            Surname = "Test",
            Email = "test@test.test",
            Password = "password",
        }, ct);

        var user = await _userRepository.Single(x => x.Id == userVm.Id, ct);
        await _desksService.CreateDesk(new DeskCreateModel
        {
            Name = "Test desk"
        }, user, ct);
    }
}