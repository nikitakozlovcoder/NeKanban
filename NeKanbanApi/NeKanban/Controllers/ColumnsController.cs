using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Constants;
using NeKanban.Controllers.Models.ColumnModels;
using NeKanban.Data.Entities;
using NeKanban.Services.Columns;
using NeKanban.Services.ViewModels;

namespace NeKanban.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class ColumnsController : BaseAuthController
{
    private readonly IColumnsService _columnsService;
    public ColumnsController(UserManager<ApplicationUser> userManager, IColumnsService columnsService) : base(userManager)
    {
        _columnsService = columnsService;
    }
    
    [HttpGet("{deskId:int}")]
    public async Task<List<ColumnVm>> GetColumns(int deskId, CancellationToken ct)
    {
        return await _columnsService.GetColumns(deskId, ct);
    }
    
    [HttpPost("{deskId:int}")]
    public async Task<List<ColumnVm>> CreateColumn(int deskId, [FromBody]ColumnCreateModel model, CancellationToken ct)
    {
        return await _columnsService.CreateColumn(deskId, model, ColumnType.General, ct);
    }
    
    [HttpDelete("{columnId:int}")]
    public async Task<List<ColumnVm>> DeleteColumn(int columnId, CancellationToken ct)
    {
        return await _columnsService.DeleteColumn(columnId, ct);
    }

}