using System.Net;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.AppMapper;
using NeKanban.Common.Attributes;
using NeKanban.Common.Entities;
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
    private readonly IAppMapper _mapper;
    public ApplicationUsersService(
        SignInManager<ApplicationUser> signInManager, 
        IRepository<ApplicationUser> userRepository,
        ITokenProviderService tokenProviderService,
        IAppMapper mapper)
    {
        _signInManager = signInManager;
        _userRepository = userRepository;
        _tokenProviderService = tokenProviderService;
        _mapper = mapper;
    }

    public async Task<ApplicationUserVm> Login(UserLoginModel userLoginModel, CancellationToken ct) 
    {
        var user = await _userRepository.FirstOrDefault(x => x.Email == userLoginModel.Email, ct);
        if (user == null || !await _signInManager.UserManager.CheckPasswordAsync(user, userLoginModel.Password))
        {
            throw new HttpStatusCodeException(HttpStatusCode.Unauthorized);
        }
       
        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        var token = _tokenProviderService.GenerateJwtToken(principal);
        var userVm = _mapper.Map<ApplicationUserVm, ApplicationUser>(user);
        userVm.Token = token;
        return userVm;
    }

    public async Task<ApplicationUserVm> Register(UserRegisterModel userRegister, CancellationToken ct)
    {
        await Create(userRegister, ct);
        return await Login(userRegister, ct);
    }

    public async Task<ApplicationUserVm> GetById(int id,  CancellationToken ct)
    {
        var user = await _userRepository.Single(x => x.Id == id, ct);
        return _mapper.Map<ApplicationUserVm, ApplicationUser>(user);
    }

    public async Task<ApplicationUser> Create(UserRegisterModel userRegister, CancellationToken ct)
    {
        var user = _mapper.Map<ApplicationUser, UserRegisterModel>(userRegister);
        var identityResult = await _signInManager.UserManager.CreateAsync(user, userRegister.Password);
        if (!identityResult.Succeeded)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, identityResult.Errors.First().Code);
        }

        return user;
    }
}