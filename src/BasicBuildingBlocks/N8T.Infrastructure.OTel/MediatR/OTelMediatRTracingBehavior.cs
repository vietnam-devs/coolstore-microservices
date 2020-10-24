using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace N8T.Infrastructure.OTel.MediatR
{
    // https://devblogs.microsoft.com/aspnet/improvements-in-net-core-3-0-for-troubleshooting-and-monitoring-distributed-apps/
    // https://vgaltes.com/post/forwarding-correlation-ids-in-aspnetcore-version-2/
    // https://github.com/SergeyKanzhelev/ot-demo-2019-11
    // feature flag: https://github.com/Bishoymly/CoreX.Extensions/blob/master/src/MicroserviceTemplate/appsettings.json#L12
    // metrics: https://github.com/Bishoymly/CoreX.Extensions/blob/master/src/CoreX.Extensions.Metrics/HomeGenerator.cs
    public class OTelMediatRTracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<OTelMediatRTracingBehavior<TRequest, TResponse>> _logger;

        public OTelMediatRTracingBehavior(ILogger<OTelMediatRTracingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation(
                "Handling {MediatRRequest} with request={MediatRRequestData} and response={MediatRResponseData}",
                nameof(OTelMediatRTracingBehavior<TRequest, TResponse>), typeof(TRequest).Name, typeof(TResponse).Name);

            using var activityListener = new DiagnosticListener(OTelMediatROptions.OTelMediatRName);

            if (!activityListener.IsEnabled() || !activityListener.IsEnabled(OTelMediatROptions.OTelMediatRName))
            {
                return await next();
            }

            var activity = new Activity($"{OTelMediatROptions.OTelMediatRName}.Execute")
                .SetIdFormat(ActivityIdFormat.W3C);

            activityListener.StartActivity(activity, request);

            try
            {
                return await next();
            }
            finally
            {
                if (activity != null)
                {
                    activityListener.StopActivity(activity, request);
                }
            }
        }
    }
}
