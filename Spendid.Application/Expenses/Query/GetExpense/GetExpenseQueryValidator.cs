using FluentValidation;

namespace Spendid.Application.Expenses.Query.GetExpense;

public class GetExpenseQueryValidator : AbstractValidator<GetExpenseQuery>
{
    public GetExpenseQueryValidator()
    {
        RuleFor(q => q.ExpenseId).NotEmpty();
    }
}
