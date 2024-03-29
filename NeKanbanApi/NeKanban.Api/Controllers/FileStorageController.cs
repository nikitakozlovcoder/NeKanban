﻿using Batteries.FileStorage.FileStorageProxies;
using Microsoft.AspNetCore.Mvc;

namespace NeKanban.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FileStorageController : Controller
{
    private readonly IFileStorageProxy _proxy;

    public FileStorageController(IFileStorageProxy proxy)
    {
        _proxy = proxy;
    }

    [HttpGet("{fileName}")]
    public async Task<ActionResult> Proxy(string fileName, CancellationToken ct)
    {
        var url = await _proxy.GetAbsoluteUrl(fileName, ct);
        return Redirect(url);
    }
}
