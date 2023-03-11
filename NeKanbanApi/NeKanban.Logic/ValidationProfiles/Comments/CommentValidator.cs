using FluentValidation;
using JetBrains.Annotations;

namespace NeKanban.Logic.ValidationProfiles.Comments;

[UsedImplicitly]
public class CommentValidator : AbstractValidator<CommentValidationModel>
{
    public CommentValidator()
    {
        RuleFor(x => x.Body).NotEmpty().MinimumLength(10);
    }
}