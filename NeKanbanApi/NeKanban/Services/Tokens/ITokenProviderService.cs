using System.Security.Claims;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.Tokens;

public interface ITokenProviderService
{
    Token GenerateJwtToken(ClaimsPrincipal principal);
}