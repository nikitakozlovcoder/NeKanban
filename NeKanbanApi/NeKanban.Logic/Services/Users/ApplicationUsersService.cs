﻿using System.Net;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.AppMapper;
using NeKanban.Common.Attributes;
using NeKanban.Common.DTOs.ApplicationUsers;
using NeKanban.Common.Entities;
using NeKanban.Common.Exceptions;
using NeKanban.Common.Models.UserModel;
using NeKanban.Common.ViewModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Services.Tokens;

namespace NeKanban.Logic.Services.Users;

[UsedImplicitly]
[Injectable<IApplicationUsersService>]
public class ApplicationUsersService : BaseService, IApplicationUsersService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly ITokenProviderService _tokenProviderService;
    private readonly IRepository<UserRefreshToken> _tokenRepository;
    private readonly IAppMapper _mapper;
    public ApplicationUsersService(
        SignInManager<ApplicationUser> signInManager, 
        IRepository<ApplicationUser> userRepository,
        ITokenProviderService tokenProviderService,
        IAppMapper mapper,
        IRepository<UserRefreshToken> tokenRepository)
    {
        _signInManager = signInManager;
        _userRepository = userRepository;
        _tokenProviderService = tokenProviderService;
        _mapper = mapper;
        _tokenRepository = tokenRepository;
    }

    public async Task<ApplicationUserWithTokenVm> Login(UserLoginModel userLoginModel, CancellationToken ct) 
    {
        var user = await _userRepository.FirstOrDefault(x => x.Email == userLoginModel.Email, ct);
        if (user == null || !await _signInManager.UserManager.CheckPasswordAsync(user, userLoginModel.Password))
        {
            throw new HttpStatusCodeException(HttpStatusCode.Unauthorized);
        }
        
        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        var token = _tokenProviderService.GenerateJwtAccessToken(principal);
        var refreshToken = _tokenProviderService.GenerateJwtRefreshToken(principal);
        await _tokenProviderService.SaveRefreshToken(user.Id, refreshToken.UniqId, refreshToken.ExpiresAt, ct);
        var userVm = _mapper.AutoMap<ApplicationUserWithTokenVm, ApplicationUser>(user);
        userVm.Token = new JwtTokenPair
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token
        };
        
        return userVm;
    }

    public async Task<ApplicationUserWithTokenVm> Register(UserRegisterModel userRegister, CancellationToken ct)
    {
        await Create(userRegister, ct);
        return await Login(userRegister, ct);
    }

    public async Task<JwtTokenPair> RefreshToken(UserRefreshTokenModel refreshTokenModel, CancellationToken ct)
    {
        var tokenData = _tokenProviderService.ReadJwtRefreshToken(refreshTokenModel.RefreshToken);
        var user = await _userRepository.Single(x => x.UserName == tokenData.UserUniqueName.ToString(), ct);
        var isValid = await _tokenProviderService.ValidateRefreshToken(user.Id, tokenData.UniqId, ct);
        if (!isValid)
        {
            throw new HttpStatusCodeException(HttpStatusCode.Unauthorized);
        }
        
        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        var accessToken = _tokenProviderService.GenerateJwtAccessToken(principal);
        var refreshToken = _tokenProviderService.GenerateJwtRefreshToken(principal);
        await _tokenProviderService.SaveRefreshToken(user.Id, refreshToken.UniqId, refreshToken.ExpiresAt, ct);

        return new JwtTokenPair
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
        };

    }

    public async Task<ApplicationUserWithTokenVm> GetById(int id,  CancellationToken ct)
    {
        var user = await _userRepository.Single(x => x.Id == id, ct);
        return _mapper.AutoMap<ApplicationUserWithTokenVm, ApplicationUser>(user);
    }

    public async Task<ApplicationUser> Create(UserRegisterModel userRegister, CancellationToken ct)
    {
        if (!userRegister.PersonalDataAgreement)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, Exceptions.PersonalDataAgreementNotAccepted);
        }
        
        var user = _mapper.AutoMap<ApplicationUser, UserRegisterModel>(userRegister);
        var identityResult = await _signInManager.UserManager.CreateAsync(user, userRegister.Password);
        if (!identityResult.Succeeded)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, identityResult.Errors.First().Code);
        }

        return user;
    }
}