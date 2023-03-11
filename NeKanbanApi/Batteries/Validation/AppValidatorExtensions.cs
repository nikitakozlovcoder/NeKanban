using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Batteries.Validation;

public static class AppValidatorExtensions
{
    public static void AddAppValidator(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddScoped(typeof(IAppValidator<>), typeof(AppValidator<>));
    }
}