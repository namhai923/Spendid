using FluentValidation;

namespace Spendid.Application.Households.Command.AddHouseholdMember;

internal sealed class AddHouseholdMemberCommandValidator : AbstractValidator<AddHouseholdMemberCommand>
{
    public AddHouseholdMemberCommandValidator()
    {
        RuleFor(c => c.AdminId).NotEmpty();

        RuleFor(c => c.HouseholdId).NotEmpty();

        RuleFor(c => c.UserEmail).NotEmpty().EmailAddress();
    }
}
