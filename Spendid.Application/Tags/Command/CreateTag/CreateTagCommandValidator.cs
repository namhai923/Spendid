using FluentValidation;

namespace Spendid.Application.Tags.Command.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(c => c.TagName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Color)
            .NotEmpty()
            .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Color must be a valid hex code (e.g., #FF5733 or #F53).");
    }
}
