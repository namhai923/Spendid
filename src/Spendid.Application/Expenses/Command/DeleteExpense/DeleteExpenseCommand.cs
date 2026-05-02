using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Expenses.Command.DeleteExpense;

public record DeleteExpenseCommand(Guid UserId, Guid ExpenseId) : ICommand;
