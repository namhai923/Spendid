using FluentValidation;

namespace Spendid.Application.Expenses.Command.MakeExpense;

internal sealed class MakeExpenseCommandValidator : AbstractValidator<MakeExpenseCommand>
{
    public MakeExpenseCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();

        RuleFor(c => c.HouseholdId).NotEmpty();

        RuleFor(c => c.Amount).GreaterThan(0);

        RuleFor(c => c.AmountCurrency).NotEmpty();

        RuleFor(c => c.Tags).NotNull();
    }
}
