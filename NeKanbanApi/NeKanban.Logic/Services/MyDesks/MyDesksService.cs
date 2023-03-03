using AutoMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.AppMapper;
using NeKanban.Common.Attributes;
using NeKanban.Common.DTOs.Desks;
using NeKanban.Common.Entities;
using NeKanban.Common.ViewModels;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Logic.Services.MyDesks;

[UsedImplicitly]
[Injectable(typeof(IMyDesksService))]
public class MyDesksService : IMyDesksService
{
    private readonly IRepository<Desk> _deskRepository;
    private readonly IAppMapper _mapper;

    public MyDesksService(IRepository<Desk> deskRepository, IAppMapper mapper)
    {
        _deskRepository = deskRepository;
        _mapper = mapper;
    }

    public async Task<List<DeskLiteDto>> GetForUser(int userId, CancellationToken ct)
    {
        var desks = await _deskRepository.QueryableSelect()
            .Include(x => x.DeskUsers.Where(du => du.UserId == userId)).ThenInclude(x => x.Role)
            .Include(x => x.DeskUsers.Where(du => du.UserId == userId)).ThenInclude(x => x.User)
            .Where(x=> x.DeskUsers.Any(du => du.UserId == userId))
            .ToListAsync(ct);
        return _mapper.AutoMap<DeskLiteDto, Desk>(desks);
    }
}