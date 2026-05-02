using FluentValidation;

namespace Spendid.Application.Households.Command.DeleteHousehold;

internal sealed class DeleteHouseholdCommandValidator : AbstractValidator<DeleteHouseholdCommand>
{
    public DeleteHouseholdCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();

        RuleFor(c => c.HouseholdId).NotEmpty();
    }
}
