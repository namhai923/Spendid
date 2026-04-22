using FluentValidation;

namespace Spendid.Application.Tags.Command.UpdateTag;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();

        RuleFor(c => c.TagId).NotEmpty();

        RuleFor(c => c.TagName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Color)
            .NotEmpty()
            .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Color must be a valid hex code (e.g., #FF5733 or #F53).");
    }
}
