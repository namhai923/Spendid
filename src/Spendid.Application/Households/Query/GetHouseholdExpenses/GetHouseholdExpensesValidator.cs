using FluentValidation;

namespace Spendid.Application.Households.Query.GetHouseholdExpenses;

internal sealed class GetHouseholdExpensesValidator : AbstractValidator<GetHouseholdExpensesQuery>
{
    public GetHouseholdExpensesValidator()
    {
        RuleFor(q => q.HouseholdId).NotEmpty();
    }
}
