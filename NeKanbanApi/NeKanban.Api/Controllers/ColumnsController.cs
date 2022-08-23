using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Controllers.Auth;
using NeKanban.Data.Constants;
using NeKanban.Data.Entities;
using NeKanban.Logic.Models.ColumnModels;
using NeKanban.Logic.Services.Columns;
using NeKanban.Logic.Services.ViewModels;
using NeKanban.Security.Constants;

namespace NeKanban.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class ColumnsController : BaseAuthController
{
    private readonly IColumnsService _columnsService;
    public ColumnsController(
        UserManager<ApplicationUser> userManager,
        IColumnsService columnsService,
        IServiceProvider serviceProvider) : base(userManager, serviceProvider)
    {
        _columnsService = columnsService;
    }
    
    [HttpGet("{deskId:int}")]
    public async Task<List<ColumnVm>> GetColumns(int deskId, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(PermissionType.AccessDesk, deskId, ct);
        return await _columnsService.GetColumns(deskId, ct);
    }
    
    [HttpPost("{deskId:int}")]
    public async Task<List<ColumnVm>> CreateColumn(int deskId, [FromBody]ColumnCreateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(PermissionType.CreateColumns, deskId, ct);
        return await _columnsService.CreateColumn(deskId, model, ColumnType.General, ct);
    }
    
    [HttpPut("{columnId:int}")]
    public async Task<List<ColumnVm>> UpdateColumn(int columnId, [FromBody]ColumnUpdateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Column>(PermissionType.ManageColumns, columnId, ct);
        return await _columnsService.UpdateColumn(columnId, model, ct);
    }
    
    [HttpPut("{columnId:int}")]
    public async Task<List<ColumnVm>> MoveColumn(int columnId, [FromBody]ColumnMoveModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Column>(PermissionType.ManageColumns, columnId, ct);
        return await _columnsService.MoveColumn(columnId, model, ct);
    }
    
    [HttpDelete("{columnId:int}")]
    public async Task<List<ColumnVm>> DeleteColumn(int columnId, CancellationToken ct)
    {
        await EnsureAbleTo<Column>(PermissionType.ManageColumns, columnId, ct);
        return await _columnsService.DeleteColumn(columnId, ct);
    }
}
