using Grpc.Core;
using GrpcJsonTranscoder.Grpc;
using GrpcJsonTranscoder.Internal.Grpc;
using GrpcJsonTranscoder.Internal.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcJsonTranscoder.Internal.Middleware
{
    internal class GrpcJsonTranscoderMiddleware
    {
        private readonly RequestDelegate _next;

        public GrpcJsonTranscoderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, GrpcAssemblyResolver grpcAssemblyResolver, IOptions<GrpcMapperOptions> options)
        {
            if (!context.Request.Headers.Any(h => h.Key.ToLowerInvariant() == "content-type" && h.Value == "application/grpc")) await _next(context);
            else
            {
                var path = context.Request.Path.Value;

                var methodDescriptor = grpcAssemblyResolver.FindMethodDescriptor(path.Split('/').Last().ToUpperInvariant());

                if (methodDescriptor == null) await _next(context);
                else
                {
                    string requestData;

                    if (context.Request.Method.ToLowerInvariant() == "get")
                    {
                        requestData = context.ParseGetJsonRequestOnAggregateService();
                    }
                    else
                    {
                        requestData = await context.ParseOtherJsonRequestOnAggregateService();
                    }

                    var grpcLookupTable = options.Value.GrpcMappers;
                    var grpcClient = grpcLookupTable.FirstOrDefault(x => x.GrpcMethod == path).GrpcHost; //todo: should catch object to throw exception

                    var channel = new Channel(grpcClient, ChannelCredentials.Insecure);
                    var client = new MethodDescriptorCaller(channel);

                    var requestObject = JsonConvert.DeserializeObject(requestData, methodDescriptor.InputType.ClrType);
                    var result = await client.InvokeAsync(methodDescriptor, context.GetRequestHeaders(), requestObject);

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                }
            }
        }
    }
}
