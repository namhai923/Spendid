using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Expenses.Command.MakeExpense;

public record MakeExpenseCommand(
    Guid UserId,
    Guid HouseholdId,
    decimal Amount,
    string AmountCurrency,
    string Description,
    List<Guid> Tags) : ICommand<Guid>;
