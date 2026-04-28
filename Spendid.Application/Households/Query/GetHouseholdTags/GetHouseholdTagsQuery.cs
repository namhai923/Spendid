using Spendid.Application.Abstractions.Caching;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;

namespace Spendid.Application.Households.Query.GetHouseholdTags;

public record GetHouseholdTagsQuery(Guid HouseholdId) : ICachedQuery<IEnumerable<TagDto>>
{
    public string CacheKey => $"GetHouseholdTags-{HouseholdId}";

    public TimeSpan? Expiration => null;
}
