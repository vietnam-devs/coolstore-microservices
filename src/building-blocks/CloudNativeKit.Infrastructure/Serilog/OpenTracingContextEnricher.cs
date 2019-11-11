using OpenTracing.Util;
using Serilog.Core;
using Serilog.Events;

namespace CloudNativeKit.Infrastructure.Serilog
{
    public class OpenTracingContextEnricher : ILogEventEnricher
    {
        public OpenTracingContextEnricher()
        {

        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var tracer = GlobalTracer.Instance;
            if (tracer?.ActiveSpan == null)
            {
                return;
            }

            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("TraceId", tracer.ActiveSpan.Context.TraceId));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("SpanId", tracer.ActiveSpan.Context.SpanId));
        }
    }
}
