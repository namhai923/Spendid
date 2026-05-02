using Spendid.Application.Abstractions.Caching;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;

namespace Spendid.Application.Households.Query.GetHouseholdExpenses;

public record GetHouseholdExpensesQuery(Guid HouseholdId) : ICachedQuery<IEnumerable<ExpenseDto>>
{
    public string CacheKey => $"GetHouseholdExpenses-{HouseholdId}";

    public TimeSpan? Expiration => null;
}
