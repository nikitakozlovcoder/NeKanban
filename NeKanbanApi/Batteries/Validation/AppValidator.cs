using System.Net;
using Batteries.Exceptions;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Batteries.Validation;

public class AppValidator<T> : IAppValidator<T>
{
    private readonly IValidator<T> _validator;

    public AppValidator(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async Task ValidateOrThrow(T obj, CancellationToken ct)
    {
        var res = await Validate(obj, ct);
        if (!res.IsValid)
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, res.ToString());
        }
    }

    public Task<ValidationResult> Validate(T obj, CancellationToken ct)
    {
        return _validator.ValidateAsync(obj, ct);
    }
}