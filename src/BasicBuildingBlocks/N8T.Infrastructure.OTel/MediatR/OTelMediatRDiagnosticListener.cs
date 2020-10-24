using System.Diagnostics;
using System.Text.Json;
using OpenTelemetry.Instrumentation;
using OpenTelemetry.Trace;

namespace N8T.Infrastructure.OTel.MediatR
{
    public class OTelMediatRDiagnosticListener : ListenerHandler
    {
        private readonly ActivitySourceAdapter _activitySource;

        public OTelMediatRDiagnosticListener(string name, ActivitySourceAdapter activitySource) : base(name)
        {
            _activitySource = activitySource;
        }

        public override void OnStartActivity(Activity activity, object payload)
        {
            // add more tags
            activity.AddTag("request", activity.OperationName);
            activity.AddTag("request.data", JsonSerializer.Serialize(payload));

            activity.SetKind(ActivityKind.Server);

            _activitySource.Start(activity);
        }

        public override void OnStopActivity(Activity activity, object payload)
        {
            _activitySource.Stop(activity);
        }
    }
}
