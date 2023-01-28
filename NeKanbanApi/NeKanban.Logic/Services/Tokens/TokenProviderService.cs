﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeKanban.Common.Attributes;
using NeKanban.Logic.Options;
using NeKanban.Logic.Services.ViewModels;

namespace NeKanban.Logic.Services.Tokens;

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