using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Controllers.Models;
using NeKanban.Data.Entities;
using NeKanban.Services.Desks;
using NeKanban.Services.ViewModels;

namespace NeKanban.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class DesksController : ControllerBase
{
    private readonly IDesksService _desksService;
    private readonly UserManager<ApplicationUser> _userManager;
    public DesksController(IDesksService desksService, UserManager<ApplicationUser> userManager)
    {
        _desksService = desksService;
        _userManager = userManager;
    }
    
    [HttpPost]
    public Task<DeskVm> CreateDesk([FromBody]DeskCreateModel deskCreateModel, CancellationToken ct = default)
    {
        return _desksService.CreateDesk(deskCreateModel, ct);
    }
    
    [HttpGet]
    public async Task<List<DeskVm>> GetForUser(CancellationToken ct = default)
    {
        var user = await _userManager.GetUserAsync(User);
        return await _desksService.GetForUser(user.Id, ct);
    }
}