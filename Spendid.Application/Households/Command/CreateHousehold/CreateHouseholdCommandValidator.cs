using FluentValidation;

namespace Spendid.Application.Households.Command.CreateHousehold;

public class CreateHouseholdCommandValidator : AbstractValidator<CreateHouseholdCommand>
{
    public CreateHouseholdCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
    }
}
