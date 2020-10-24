using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace N8T.Infrastructure.Validator
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;
        private readonly ILogger<RequestValidationBehavior<TRequest, TResponse>> _logger;

        public RequestValidationBehavior(IValidator<TRequest> validator,
            ILogger<RequestValidationBehavior<TRequest, TResponse>> logger)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation($"Handling {typeof(TRequest).FullName}");
            _logger.LogDebug($"Handling {typeof(TRequest).FullName} with content {JsonSerializer.Serialize(request)}");
            await _validator.HandleValidation(request);
            var response = await next();
            _logger.LogInformation($"Handled {typeof(TRequest).FullName}");
            return response;
        }
    }
}