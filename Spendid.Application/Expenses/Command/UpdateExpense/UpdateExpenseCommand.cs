using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Expenses.Command.UpdateExpense;

public record UpdateExpenseCommand(
    Guid UserId,
    Guid ExpenseId,
    decimal Amount,
    string AmountCurrency,
    string Description,
    List<Guid> Tags) : ICommand<Guid>;
