using Batteries.FileStorage.FileStorageProxies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NeKanban.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class FileStorageController : Controller
{
    private readonly IFileStorageProxy _proxy;

    public FileStorageController(IFileStorageProxy proxy)
    {
        _proxy = proxy;
    }

    [HttpGet("{fileId:guid}")]
    public async Task<ActionResult> Proxy(Guid fileId, CancellationToken ct)
    {
        var url = await _proxy.GetAbsoluteUrl(fileId, ct);
        return Redirect(url);
    }
}
