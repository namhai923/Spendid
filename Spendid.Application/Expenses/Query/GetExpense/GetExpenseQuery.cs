using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;

namespace Spendid.Application.Expenses.Query.GetExpense;

public record GetExpenseQuery(Guid ExpenseId) : IQuery<ExpenseDto>;
