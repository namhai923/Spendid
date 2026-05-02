using FluentValidation;

namespace Spendid.Application.Tags.Command.DeleteTag;

internal sealed class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();

        RuleFor(c => c.TagId).NotEmpty();
    }
}
