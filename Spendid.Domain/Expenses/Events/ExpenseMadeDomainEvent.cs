using Spendid.Domain.Abstractions;

namespace Spendid.Domain.Expenses.Events;

public sealed record ExpenseMadeDomainEvent(Guid ExpenseId) : IDomainEvent;
