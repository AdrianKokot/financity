using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Financity.Application.Behaviors;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        _logger.LogInformation("Request: {Name} {@Request}", requestName, request);
    }
}