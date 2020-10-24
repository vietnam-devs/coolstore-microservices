using System;
using OpenTelemetry.Instrumentation;
using OpenTelemetry.Trace;

namespace N8T.Infrastructure.OTel.MediatR
{
    public class OTelMediatRInstrumentation : IDisposable
    {
        private readonly DiagnosticSourceSubscriber _diagnosticSourceSubscriber;

        public OTelMediatRInstrumentation(ActivitySourceAdapter activitySource)
        {
            if (activitySource == null)
            {
                throw new ArgumentNullException(nameof(activitySource));
            }

            _diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(
                name => new OTelMediatRDiagnosticListener(OTelMediatROptions.OTelMediatRName, activitySource),
                listener =>
                {
                    return listener.Name.Contains(OTelMediatROptions.OTelMediatRName);
                },
                null
            );

            _diagnosticSourceSubscriber?.Subscribe();
        }

        public void Dispose()
        {
            _diagnosticSourceSubscriber?.Dispose();
        }
    }
}
