using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Serilog;

namespace CloudNativeKit.Infrastructure.Grpc
{
    public class RequestLoggerInterceptor : Interceptor
    {
        private const string MessageTemplate = "{RequestMethod} responded {StatusCode} in {Elapsed:0.0000} ms";

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var sw = Stopwatch.StartNew();
            var response = await base.UnaryServerHandler(request, context, continuation);
            sw.Stop();

            Log.Logger.Information(MessageTemplate,
              context.Method,
              context.Status.StatusCode,
              sw.Elapsed.TotalMilliseconds);

            return response;
        }
    }
}
