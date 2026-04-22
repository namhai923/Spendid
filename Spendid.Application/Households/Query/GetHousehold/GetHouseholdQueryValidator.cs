using FluentValidation;

namespace Spendid.Application.Households.Query.GetHousehold;

public class GetHouseholdQueryValidator : AbstractValidator<GetHouseholdQuery>
{
    public GetHouseholdQueryValidator()
    {
        RuleFor(q => q.HouseholdId).NotEmpty();
    }
}
