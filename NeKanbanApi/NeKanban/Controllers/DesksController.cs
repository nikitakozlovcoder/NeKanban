using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Constants.Security;
using NeKanban.Controllers.Models;
using NeKanban.Controllers.Models.DeskModels;
using NeKanban.Data.Entities;
using NeKanban.Services.Desks;
using NeKanban.Services.MyDesks;
using NeKanban.Services.ViewModels;

namespace NeKanban.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class DesksController : BaseAuthController
{
    private readonly IDesksService _desksService;
    private readonly IMyDesksService _myDesksService;
    public DesksController(IDesksService desksService, UserManager<ApplicationUser> userManager, 
        IMyDesksService myDesksService,
        IServiceProvider serviceProvider) : base(userManager, serviceProvider)
    {
        _desksService = desksService;
        _myDesksService = myDesksService;
    }
    
    [HttpPost]
    public Task<DeskVm> CreateDesk([FromBody]DeskCreateModel deskCreateModel, CancellationToken ct = default)
    {
        return _desksService.CreateDesk(deskCreateModel, ct);
    }
    
    [HttpPut("{id:int}")]
    public async Task<DeskVm> UpdateDesk([FromBody]DeskUpdateModel deskUpdateModel, int id, CancellationToken ct = default)
    {
        await EnsureAbleTo<Desk>(PermissionType.UpdateGeneralDesk, id, ct);
        return await _desksService.UpdateDesk(deskUpdateModel, id, ct);
    }
    
    [HttpPut("{id:int}")]
    public async Task<DeskVm> InviteLink([FromBody]DeskInviteLinkModel inviteLinkModel, int id, CancellationToken ct = default)
    {
        await EnsureAbleTo<Desk>(PermissionType.ManageInviteLink, id, ct);
        return await _desksService.UpdateDesk(inviteLinkModel, id, ct);
    }
    
    [HttpGet("{id:int}")]
    public async Task<DeskVm> GetDesk(int id, CancellationToken ct = default)
    {
        await EnsureAbleTo<Desk>(PermissionType.AccessDesk, id, ct);
        return await _desksService.GetDesk(id, ct);
    }

    [HttpGet]
    public async Task<List<DeskLightVm>> GetForUser(CancellationToken ct = default)
    {
        var user = await GetApplicationUser();
        return await _myDesksService.GetForUser(user.Id, ct);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        await EnsureAbleTo<Desk>(PermissionType.DeleteDesk, id, ct);
        await _desksService.DeleteDesk(id, ct);
        return Ok();
    }
}