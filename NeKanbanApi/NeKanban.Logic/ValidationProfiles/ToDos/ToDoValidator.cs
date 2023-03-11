using FluentValidation;
using JetBrains.Annotations;

namespace NeKanban.Logic.ValidationProfiles.ToDos;

[UsedImplicitly]
public class ToDoValidator : AbstractValidator<ToDoValidationModel>
{
    public ToDoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
    }
}