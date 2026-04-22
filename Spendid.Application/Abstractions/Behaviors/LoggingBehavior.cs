using MediatR;
using Microsoft.Extensions.Logging;
using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var commandName = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing command {commandName}", commandName);

            var result = await next(cancellationToken);

            _logger.LogInformation("Command {commandName} processed successfully", commandName);

            return result;
        }
        catch(Exception exception)
        {
            _logger.LogError(exception, "Command {commandName} processing failed", commandName);

            throw;
        }
    }
}
