using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Attributes;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Mappings;
using NeKanban.Logic.Services.ViewModels;
using NeKanban.Services.MyDesks;

namespace NeKanban.Logic.Services.MyDesks;

[Injectable(typeof(IMyDesksService))]
public class MyDesksService : IMyDesksService
{
    private readonly IRepository<Desk> _deskRepository;

    public MyDesksService(IRepository<Desk> deskRepository)
    {
        _deskRepository = deskRepository;
    }

    public async Task<List<DeskLiteVm>> GetForUser(int userId, CancellationToken ct)
    {
        var desks = await _deskRepository.QueryableSelect()
            .Include(x => x.DeskUsers.Where(du => du.UserId == userId)).ThenInclude(x=> x.User)
            .Where(x=> x.DeskUsers.Any(du => du.UserId == userId))
            .ToListAsync(ct);
        return desks.Select(x => x.ToDeskLiteVm()).ToList();
    }
}