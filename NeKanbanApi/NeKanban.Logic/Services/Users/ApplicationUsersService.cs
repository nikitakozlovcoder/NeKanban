using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.Attributes;
using NeKanban.Common.Entities;
using NeKanban.Common.Models.UserModel;
using NeKanban.Common.ViewModels;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Mappings;
using NeKanban.Logic.Services.Tokens;

namespace NeKanban.Logic.Services.Users;

[Injectable<IApplicationUsersService>]
public class ApplicationUsersService : BaseService, IApplicationUsersService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly ITokenProviderService _tokenProviderService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    public ApplicationUsersService(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        IRepository<ApplicationUser> userRepository,
        ITokenProviderService tokenProviderService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _tokenProviderService = tokenProviderService;
        _mapper = mapper;
    }

    public async Task<ApplicationUserVm> Login(UserLoginModel userLoginModel, CancellationToken ct) 
    {
        var user = await _userRepository.FirstOrDefault(x => x.Email == userLoginModel.Email, ct);
        if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginModel.Password))
        {
            throw new HttpStatusCodeException(HttpStatusCode.Unauthorized);
        }

        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        var token = _tokenProviderService.GenerateJwtToken(principal);
        var userVm = _mapper.Map<ApplicationUserVm>(user);
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
        var user = await _userRepository.FirstOrDefault(x => x.Id == id, ct);
        EnsureEntityExists(user);
        
        return _mapper.Map<ApplicationUserVm>(user);
    }

    public async Task<ApplicationUser> Create(UserRegisterModel userRegister, CancellationToken ct)
    {
        var user = _mapper.Map<ApplicationUser>(userRegister);
        var identityResult = await _userManager.CreateAsync(user, userRegister.Password);
        if (!identityResult.Succeeded)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, identityResult.Errors.First().Code);
        }

        return user;
    }
}