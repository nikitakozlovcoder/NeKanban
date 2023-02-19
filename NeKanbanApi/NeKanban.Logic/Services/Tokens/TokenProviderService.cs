using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Batteries.Injection.Attributes;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeKanban.Common.ViewModels;
using NeKanban.Logic.Options;

namespace NeKanban.Logic.Services.Tokens;

[UsedImplicitly]
[Injectable(typeof(ITokenProviderService))]
public class TokenProviderService : ITokenProviderService
{
    private readonly JwtSettings _jwtSettings;
    public TokenProviderService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public Token GenerateJwtToken(ClaimsPrincipal principal)
    {
        var handler = new JwtSecurityTokenHandler();
        var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret!));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Subject = new ClaimsIdentity(principal.Identity),
            Expires = DateTime.UtcNow.AddDays(365),
            SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        return new Token
        {
            TokenValue = handler.WriteToken(token)
        };
    }
}