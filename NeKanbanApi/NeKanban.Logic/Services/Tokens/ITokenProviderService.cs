﻿using System.Security.Claims;
using NeKanban.Logic.Services.ViewModels;

namespace NeKanban.Logic.Services.Tokens;

public interface ITokenProviderService
{
    Token GenerateJwtToken(ClaimsPrincipal principal);
}