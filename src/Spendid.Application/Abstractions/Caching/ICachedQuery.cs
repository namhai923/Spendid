using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Abstractions.Caching;

public interface ICachedQuery<TRequest> : IQuery<TRequest>, ICachedQuery;

public interface ICachedQuery
{
    string CacheKey { get; }

    TimeSpan? Expiration { get; }
}
