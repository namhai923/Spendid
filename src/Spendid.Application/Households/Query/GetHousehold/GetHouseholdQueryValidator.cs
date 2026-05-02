using FluentValidation;

namespace Spendid.Application.Households.Query.GetHousehold;

internal sealed class GetHouseholdQueryValidator : AbstractValidator<GetHouseholdQuery>
{
    public GetHouseholdQueryValidator()
    {
        RuleFor(q => q.HouseholdId).NotEmpty();
    }
}
