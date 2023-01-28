using System.Net;
using Microsoft.AspNetCore.Identity;
using NeKanban.Api.FrameworkExceptions.ExceptionHandling;
using NeKanban.Common.Attributes;
using NeKanban.Data.Entities;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Mappings;
using NeKanban.Logic.Models.UserModel;
using NeKanban.Logic.Services.Tokens;
using NeKanban.Logic.Services.ViewModels;

namespace NeKanban.Logic.Services.Users;

[Injectable(typeof(ApplicationUsersService))]
public class ApplicationUsersService : BaseService, IApplicationUsersService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly ITokenProviderService _tokenProviderService;
    private readonly UserManager<ApplicationUser> _userManager;
    public ApplicationUsersService(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        IRepository<ApplicationUser> userRepository, ITokenProviderService tokenProviderService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _tokenProviderService = tokenProviderService;
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
        return user.ToApplicationUserVm(token);
    }

    public async Task<ApplicationUserVm> Register(UserRegisterModel userRegister, CancellationToken ct)
    {
        var user = new ApplicationUser();
        user.FromRegistrationModel(userRegister);
        var identityResult = await _userManager.CreateAsync(user, userRegister.Password);
        if (!identityResult.Succeeded)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, identityResult.Errors.First().Code);
        }
        
        return await Login(userRegister, ct);
    }

    public async Task<ApplicationUserVm> GetById(int id,  CancellationToken ct)
    {
        var user = await _userRepository.FirstOrDefault(x => x.Id == id, ct);
        EnsureEntityExists(user);
        
        return user!.ToApplicationUserVm();
    }
}