using System;
using OpenTelemetry.Instrumentation;
using OpenTelemetry.Trace;

namespace N8T.Infrastructure.OTel.MediatR
{
    public class OTelMediatRInstrumentation : IDisposable
    {
        private readonly DiagnosticSourceSubscriber _diagnosticSourceSubscriber;

        public OTelMediatRInstrumentation(ActivitySourceAdapter activitySourceAdapter)
        {
            _diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(
                name => new OTelMediatRDiagnosticListener(OTelMediatROptions.OTelMediatRName, activitySourceAdapter),
                listener =>
                {
                    return listener.Name == OTelMediatROptions.OTelMediatRName;
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
