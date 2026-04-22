using FluentValidation;

namespace Spendid.Application.Households.Command.ChangeHouseholdAdmin;

public class ChangeHouseholdAdminValidator : AbstractValidator<ChangeHouseholdAdminCommand>
{
    public ChangeHouseholdAdminValidator()
    {
        RuleFor(c => c.AdminId).NotEmpty();

        RuleFor(c => c.HouseholdId).NotEmpty();

        RuleFor(c => c.UserEmail).NotEmpty().EmailAddress();
    }

}
