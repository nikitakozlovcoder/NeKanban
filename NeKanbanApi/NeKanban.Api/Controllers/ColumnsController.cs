using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeKanban.Common.Constants;
using NeKanban.Common.DTOs.Columns;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.ColumnModels;
using NeKanban.Common.ViewModels;
using NeKanban.Common.ViewModels.Columns;
using NeKanban.Controllers.Auth;
using NeKanban.Logic.Services.Columns;
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
    public async Task<List<ColumnDto>> GetColumns(int deskId, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(deskId, ct);
        return await _columnsService.GetColumns(deskId, ct);
    }
    
    [HttpPost("{deskId:int}")]
    public async Task<List<ColumnDto>> CreateColumn(int deskId, [FromBody]ColumnCreateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Desk>(PermissionType.CreateColumns, deskId, ct);
        return await _columnsService.CreateColumn(deskId, model, ColumnType.General, ct);
    }
    
    [HttpPut("{columnId:int}")]
    public async Task<List<ColumnDto>> UpdateColumn(int columnId, [FromBody]ColumnUpdateModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Column>(PermissionType.ManageColumns, columnId, ct);
        return await _columnsService.UpdateColumn(columnId, model, ct);
    }
    
    [HttpPut("{columnId:int}")]
    public async Task<List<ColumnDto>> MoveColumn(int columnId, [FromBody]ColumnMoveModel model, CancellationToken ct)
    {
        await EnsureAbleTo<Column>(PermissionType.ManageColumns, columnId, ct);
        return await _columnsService.MoveColumn(columnId, model, ct);
    }
    
    [HttpDelete("{columnId:int}")]
    public async Task<List<ColumnDto>> DeleteColumn(int columnId, CancellationToken ct)
    {
        await EnsureAbleTo<Column>(PermissionType.ManageColumns, columnId, ct);
        return await _columnsService.DeleteColumn(columnId, ct);
    }
}
