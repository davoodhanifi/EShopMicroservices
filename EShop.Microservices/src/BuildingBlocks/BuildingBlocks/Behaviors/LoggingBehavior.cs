using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) :
                                                    IPipelineBehavior<TRequest, TResponse>
                                                    where TRequest : notnull, IRequest<TResponse>
                                                    where TResponse : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var responseName = typeof(TResponse).Name;

        _logger.LogInformation($"[START] Handle Request={requestName}, Response={responseName} - Request={request}");

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();

        var timeElapsed = timer.Elapsed;

        if (timeElapsed.Seconds > 3)
        {
            _logger.LogWarning($"[PERFORMANCE] The request {requestName} took {timeElapsed.Seconds}");
        }

        _logger.LogInformation($"[END] Handled {requestName} with {responseName}");
        return response;

    }
}
