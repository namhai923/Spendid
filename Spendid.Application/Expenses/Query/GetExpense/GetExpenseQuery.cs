using Spendid.Application.Abstractions.Caching;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;

namespace Spendid.Application.Expenses.Query.GetExpense;

public record GetExpenseQuery(Guid ExpenseId) : ICachedQuery<ExpenseDto>
{
    public string CacheKey => $"GetExpense-{ExpenseId}";

    public TimeSpan? Expiration => null;
}
