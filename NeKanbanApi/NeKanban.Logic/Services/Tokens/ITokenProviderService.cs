using System.Security.Claims;
using NeKanban.Common.DTOs.RefreshTokens;

namespace NeKanban.Logic.Services.Tokens;

public interface ITokenProviderService
{
    string GenerateJwtAccessToken(ClaimsPrincipal principal);
    RefreshTokenDto GenerateJwtRefreshToken(ClaimsPrincipal principal);
    RefreshTokenReadDto ReadJwtRefreshToken(string refreshToken);
    Task SaveRefreshToken(int userId, Guid uniqId, DateTime expiresAt, CancellationToken ct);
    Task<bool> ValidateRefreshToken(int userId, Guid uniqId, CancellationToken ct);
}