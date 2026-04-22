using FluentValidation;

namespace Spendid.Application.Households.Query.GetHouseholdExpenses;

public class GetHouseholdExpensesValidator : AbstractValidator<GetHouseholdExpensesQuery>
{
    public GetHouseholdExpensesValidator()
    {
        RuleFor(q => q.HouseholdId).NotEmpty();
    }
}
