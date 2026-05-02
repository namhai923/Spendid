using FluentValidation;

namespace Spendid.Application.Expenses.Query.GetExpense;

internal sealed class GetExpenseQueryValidator : AbstractValidator<GetExpenseQuery>
{
    public GetExpenseQueryValidator()
    {
        RuleFor(q => q.ExpenseId).NotEmpty();
    }
}
