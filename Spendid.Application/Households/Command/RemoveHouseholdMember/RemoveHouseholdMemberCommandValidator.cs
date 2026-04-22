using FluentValidation;

namespace Spendid.Application.Households.Command.RemoveHouseholdMember;

public class RemoveHouseholdMemberCommandValidator : AbstractValidator<RemoveHouseholdMemberCommand>
{
    public RemoveHouseholdMemberCommandValidator()
    {
        RuleFor(c => c.AdminId).NotEmpty();

        RuleFor(c => c.HouseholdId).NotEmpty();

        RuleFor(c => c.UserEmail).NotEmpty().EmailAddress();
    }
}
