using FluentValidation;

namespace Spendid.Application.Expenses.Command.UpdateExpense;

public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();

        RuleFor(c => c.ExpenseId).NotEmpty();

        RuleFor(c => c.Amount).GreaterThan(0);

        RuleFor(c => c.AmountCurrency).NotEmpty();

        RuleFor(c => c.Tags).NotNull();
    }
}
