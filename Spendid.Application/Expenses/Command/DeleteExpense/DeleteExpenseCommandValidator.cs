using FluentValidation;

namespace Spendid.Application.Expenses.Command.DeleteExpense;

public class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();

        RuleFor(c => c.ExpenseId).NotEmpty();
    }
}
