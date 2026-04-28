using Spendid.Application.Abstractions.Caching;
using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Households.Query.GetHouseholdUsers;

public record GetHouseholdUsersQuery(Guid HouseholdId) : ICachedQuery<IEnumerable<GetHouseholdUsersQueryResponse>>
{
    public string CacheKey => $"GetHouseholdUsers-{HouseholdId}";

    public TimeSpan? Expiration => null;
}
