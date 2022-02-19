using System.Net;
using Microsoft.AspNetCore.Identity;
using NeKanban.Controllers.Models;
using NeKanban.Controllers.Models.UserModel;
using NeKanban.Data;
using NeKanban.Data.Entities;
using NeKanban.ExceptionHandling;
using NeKanban.Mappings;
using NeKanban.Services.Tokens;
using NeKanban.Services.ViewModels;

namespace NeKanban.Services.Users;

public class ApplicationUsersService : BaseService, IApplicationUsersService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly ITokenProviderService _tokenProviderService;
    public ApplicationUsersService(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        IRepository<ApplicationUser> userRepository, ITokenProviderService tokenProviderService) : base(userManager)
    {
        _signInManager = signInManager;
        _userRepository = userRepository;
        _tokenProviderService = tokenProviderService;
    }

    public async Task<ApplicationUserVm> Login<T>(T userLoginModel, CancellationToken ct) where T : UserLoginModel
    {
        var user = await _userRepository.GetFirstOrDefault(x => x.Email == userLoginModel.Email, ct);
        if (user == null || !await UserManager.CheckPasswordAsync(user, userLoginModel.Password))
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
        var identityResult = await UserManager.CreateAsync(user, userRegister.Password);
        if (!identityResult.Succeeded)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, identityResult.Errors.First().Code);
        }
        return await Login(userRegister, ct);
    }

    public async Task<ApplicationUserVm> GetById(int id,  CancellationToken ct)
    {
        var user = await _userRepository.GetFirstOrDefault(x => x.Id == id, ct);
        if (user == null)
        {
            throw new HttpStatusCodeException(HttpStatusCode.NotFound);
        }
        return user.ToApplicationUserVm();
    }
}