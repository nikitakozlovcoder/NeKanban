﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Attributes;
using NeKanban.Common.Entities;
using NeKanban.Common.ViewModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Mappings;

namespace NeKanban.Logic.Services.MyDesks;

[Injectable(typeof(IMyDesksService))]
public class MyDesksService : IMyDesksService
{
    private readonly IRepository<Desk> _deskRepository;
    private readonly IMapper _mapper;

    public MyDesksService(IRepository<Desk> deskRepository, IMapper mapper)
    {
        _deskRepository = deskRepository;
        _mapper = mapper;
    }

    public async Task<List<DeskLiteVm>> GetForUser(int userId, CancellationToken ct)
    {
        var desks = await _deskRepository.QueryableSelect()
            .Include(x => x.DeskUsers.Where(du => du.UserId == userId)).ThenInclude(x=> x.User)
            .Where(x=> x.DeskUsers.Any(du => du.UserId == userId))
            .ToListAsync(ct);
        return _mapper.Map<List<DeskLiteVm>>(desks);
    }
}