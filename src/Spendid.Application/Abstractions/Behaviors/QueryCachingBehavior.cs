using MediatR;
using Microsoft.Extensions.Logging;
using Spendid.Application.Abstractions.Caching;
using Spendid.Domain.Abstractions;

namespace Spendid.Application.Abstractions.Behaviors;

internal sealed class QueryCachingBehavior<TRequest, TResponse>(ICacheService cacheService, ILogger<QueryCachingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICachedQuery where TResponse : Result
{
    private readonly ICacheService _cacheService = cacheService;
    private readonly ILogger<QueryCachingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cachedResponse = await _cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);

        var queryName = typeof(TRequest).Name;

        if (cachedResponse is not null)
        {
            _logger.LogInformation("Cache hit for {queryName}", queryName);

            return cachedResponse;
        }

        _logger.LogInformation("Cache miss for {queryName}", queryName);

        var result = await next(cancellationToken);

        if (result.IsSuccess)
        {
            await _cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);
        }

        return result;
    }
}
