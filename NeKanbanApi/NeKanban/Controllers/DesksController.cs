using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Controllers.Models;
using NeKanban.Services.Desks;
using NeKanban.Services.ViewModels;

namespace NeKanban.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class DesksController : ControllerBase
{
    private readonly IDesksService _desksService;

    public DesksController(IDesksService desksService)
    {
        _desksService = desksService;
    }
    
    [HttpPost]
    public Task<DeskVm> CreateDesk([FromBody]DeskCreateModel deskCreateModel, CancellationToken ct = default)
    {
        return _desksService.CreateDesk(deskCreateModel, ct);
    }
}