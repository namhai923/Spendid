using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Serilog.Context;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;

namespace Spendid.Application.Abstractions.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing request {requestName}", requestName);

            var result = await next(cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Request {requestName} processed successfully", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    _logger.LogError("Request {requestName} processed with error", requestName);
                }
            }


            return result;
        }
        catch(Exception exception)
        {
            _logger.LogError(exception, "Request {requestName} processing failed", requestName);

            throw;
        }
    }
}
