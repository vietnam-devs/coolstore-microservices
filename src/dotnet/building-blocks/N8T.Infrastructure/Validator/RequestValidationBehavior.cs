using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace N8T.Infrastructure.Validator
{
    [DebuggerStepThrough]
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<RequestValidationBehavior<TRequest, TResponse>> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RequestValidationBehavior(IServiceProvider serviceProvider,
            ILogger<RequestValidationBehavior<TRequest, TResponse>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation(
                "[{Prefix}] Handle request={X-RequestData} and response={X-ResponseData}",
                nameof(RequestValidationBehavior<TRequest, TResponse>), typeof(TRequest).Name, typeof(TResponse).Name);

            _logger.LogDebug($"Handling {typeof(TRequest).FullName} with content {JsonSerializer.Serialize(request)}");

            var validator = _serviceProvider.GetService<IValidator<TRequest>>();
            if (validator is not null)
            {
                await validator.HandleValidation(request);
            }

            var response = await next();

            _logger.LogInformation($"Handled {typeof(TRequest).FullName}");
            return response;
        }
    }
}
