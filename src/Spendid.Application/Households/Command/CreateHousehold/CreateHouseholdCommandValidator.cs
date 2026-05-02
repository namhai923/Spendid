using FluentValidation;

namespace Spendid.Application.Households.Command.CreateHousehold;

internal sealed class CreateHouseholdCommandValidator : AbstractValidator<CreateHouseholdCommand>
{
    public CreateHouseholdCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
    }
}
