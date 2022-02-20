using Microsoft.EntityFrameworkCore;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.Mappings;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.MyDesks;

public class MyDesksService : IMyDesksService
{
    private readonly IRepository<Desk> _deskRepository;

    public MyDesksService(IRepository<Desk> deskRepository)
    {
        _deskRepository = deskRepository;
    }

    public async Task<List<DeskLightVm>> GetForUser(int userId, CancellationToken ct)
    {
        var desks = await _deskRepository.QueryableSelect()
            .Include(x => x.DeskUsers.Where(du => du.UserId == userId)).ThenInclude(x=> x.User)
            .Where(x=> x.DeskUsers.Any(du => du.UserId == userId))
            .ToListAsync(ct);
        return desks.Select(x => x.ToDeskLightVm()).ToList();
    }
}