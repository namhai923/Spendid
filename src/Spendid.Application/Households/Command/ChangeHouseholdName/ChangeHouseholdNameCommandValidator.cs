using FluentValidation;

namespace Spendid.Application.Households.Command.ChangeHouseholdName;

internal sealed class ChangeHouseholdNameCommandValidator : AbstractValidator<ChangeHouseholdNameCommand>
{
    public ChangeHouseholdNameCommandValidator()
    {
        RuleFor(c => c.AdminId).NotEmpty();

        RuleFor(c => c.HouseholdId).NotEmpty();
    }
}
