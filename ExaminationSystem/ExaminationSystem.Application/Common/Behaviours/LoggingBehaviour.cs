using ExaminationSystem.Application.Common.Interfaces;

using MediatR.Pipeline;

using Microsoft.Extensions.Logging;

namespace ExaminationSystem.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, IUserService user)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger = logger;
    private readonly IUserService _user = user;
  

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user.GetUserId();
        string? userName = string.Empty;

        _logger.LogInformation(
            "Request: {Name} {@UserId} {@Request}", requestName, userId, request);
    }
}