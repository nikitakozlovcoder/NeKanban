using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Batteries.Validation;

public interface IAppValidator<T>
{
    Task ValidateOrThrow(T obj, CancellationToken ct);
    Task<ValidationResult> Validate(T obj, CancellationToken ct);
}