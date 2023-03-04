using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeKanban.Common.Attributes;
using NeKanban.Common.DTOs.RefreshTokens;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Options;

namespace NeKanban.Logic.Services.Tokens;

[UsedImplicitly]
[Injectable(typeof(ITokenProviderService))]
public class TokenProviderService : ITokenProviderService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IRepository<UserRefreshToken> _tokenRepository;
    public TokenProviderService(IOptions<JwtSettings> jwtSettings, IRepository<UserRefreshToken> tokenRepository)
    {
        _tokenRepository = tokenRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateJwtAccessToken(ClaimsPrincipal principal)
    {
        var handler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(principal.Identity),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.Minutes),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    public RefreshTokenDto GenerateJwtRefreshToken(ClaimsPrincipal principal)
    {
        var handler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.RefreshSecret));
        var uniqId = Guid.NewGuid();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = new Dictionary<string, object>
            {
                {"uniqId", uniqId}
            },
            Subject = new ClaimsIdentity(principal.Identity),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshMinutes),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        return new RefreshTokenDto
        {
            Token = handler.WriteToken(token),
            UniqId = uniqId,
            ExpiresAt = tokenDescriptor.Expires.Value
        };
    }

    public RefreshTokenReadDto ReadJwtRefreshToken(string refreshToken)
    {
        var validationParams = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.FromMinutes(_jwtSettings.ClockSkewMinutes),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.RefreshSecret))
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(refreshToken, validationParams, out _);
        return new RefreshTokenReadDto
        {
            UniqId = Guid.Parse(principal.Claims.Single(x => x.Type == "uniqId").Value),
            UserUniqueName = Guid.Parse(principal.Claims.Single(x => x.Type == ClaimTypes.Name).Value)
        };
    }
    
    public Task SaveRefreshToken(int userId, Guid uniqId, DateTime expiresAt, CancellationToken ct)
    {
        return _tokenRepository.Create(new UserRefreshToken
        {
            Token = uniqId,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow,
            ApplicationUserId = userId
        }, ct);
    }

    public Task<bool> ValidateRefreshToken(int userId, Guid uniqId, CancellationToken ct)
    {
        return _tokenRepository.Any(x => x.ApplicationUserId == userId
                                         && x.Token == uniqId &&
                                         x.ExpiresAt >= DateTime.UtcNow.AddMinutes(-_jwtSettings.ClockSkewMinutes), ct);
    }
}