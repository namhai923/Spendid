using Spendid.Application.Abstractions.Caching;
using Spendid.Application.DTOs;

namespace Spendid.Application.Households.Query.GetHousehold;

public record GetHouseholdQuery(Guid HouseholdId) : ICachedQuery<HouseholdDto>
{
    public string CacheKey => $"GetHousehold-{HouseholdId}";

    public TimeSpan? Expiration => null;
}
