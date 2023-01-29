using System.Security.Claims;
using NeKanban.Common.ViewModels;

namespace NeKanban.Logic.Services.Tokens;

public interface ITokenProviderService
{
    Token GenerateJwtToken(ClaimsPrincipal principal);
}