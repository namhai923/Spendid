using Spendid.Application.Abstractions.Caching;

namespace Spendid.Application.Users.Query.GetMemberships;

public record GetMembershipsQuery(Guid UserId) : ICachedQuery<IEnumerable<GetMembershipQueryResponse>>
{
    public string CacheKey => $"GetMemberships-{UserId}";

    public TimeSpan? Expiration => null;
}
