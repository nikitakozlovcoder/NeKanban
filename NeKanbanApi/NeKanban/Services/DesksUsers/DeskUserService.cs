using System.Net;
using Microsoft.EntityFrameworkCore;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.ExceptionHandling;
using NeKanban.Mappings;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.DesksUsers;

public class DeskUserService : IDeskUserService
{
    private readonly IRepository<DeskUser> _deskUserRepository;

    public DeskUserService(IRepository<DeskUser> deskUserRepository)
    {
        _deskUserRepository = deskUserRepository;
    }

    public Task CreateDeskUser(int deskId, int userId, CancellationToken ct)
    {

        return _deskUserRepository.Create(new DeskUser
        {
            DeskId = deskId,
            UserId = userId
        }, ct);
    }

    public async Task<DeskUserVm> GetDeskUser(int id, CancellationToken ct)
    {
        var deskUser = await _deskUserRepository.QueryableSelect()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (deskUser == null)
        {
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, nameof(deskUser));
        }

        return deskUser.ToDeskUserVm();
    }
}