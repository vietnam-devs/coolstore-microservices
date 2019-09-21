using Grpc.Core;
using GrpcJsonTranscoder.Grpc;
using GrpcJsonTranscoder.Internal.Grpc;
using GrpcJsonTranscoder.Internal.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ocelot.LoadBalancer.LoadBalancers;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcJsonTranscoder
{
    public static class DownStreamContextExtensions
    {
        public static async Task HandleGrpcRequestAsync(this DownstreamContext context, Func<Task> next, SslCredentials secureCredentials = null)
        {
            // ignore if the request is not a gRPC content type
            if (!context.HttpContext.Request.Headers.Any(h => h.Key.ToLowerInvariant() == "content-type" && h.Value == "application/grpc"))
            {
                await next.Invoke();
            }
            else
            {
                var methodPath = context.DownstreamReRoute.DownstreamPathTemplate.Value;
                var grpcAssemblyResolver = context.HttpContext.RequestServices.GetService<GrpcAssemblyResolver>();
                var methodDescriptor = grpcAssemblyResolver.FindMethodDescriptor(methodPath.Split('/').Last().ToUpperInvariant());

                if (methodDescriptor == null)
                {
                    await next.Invoke();
                }
                else
                {
                    var logger = context.HttpContext.RequestServices.GetService<IOcelotLoggerFactory>().CreateLogger<GrpcAssemblyResolver>();

                    string requestData;
                    var upstreamHeaders = new Dictionary<string, string>
                            {
                                { "x-grpc-route-data", JsonConvert.SerializeObject(context.TemplatePlaceholderNameAndValues.Select(x => new {x.Name, x.Value})) },
                                { "x-grpc-body-data", await context.DownstreamRequest.Content.ReadAsStringAsync() }
                            };

                    logger.LogInformation($"Upstream header data for x-grpc-route-data: {upstreamHeaders["x-grpc-route-data"]}");
                    logger.LogInformation($"Upstream header data for x-grpc-body-data: {upstreamHeaders["x-grpc-body-data"]}");

                    if (context.HttpContext.Request.Method.ToLowerInvariant() == "get")
                    {
                        requestData = context.HttpContext.ParseGetJsonRequest(upstreamHeaders);
                        logger.LogInformation($"GET Request object data: {requestData}");
                    }
                    else
                    {
                        requestData = context.HttpContext.ParseOtherJsonRequest(upstreamHeaders);
                        logger.LogInformation($"POST/PUT/DELETE Request object data: {requestData}");
                    }

                    var loadBalancerFactory = context.HttpContext.RequestServices.GetService<ILoadBalancerFactory>();
                    var loadBalancerResponse = await loadBalancerFactory.Get(context.DownstreamReRoute, context.Configuration.ServiceProviderConfiguration);
                    var serviceHostPort = await loadBalancerResponse.Data.Lease(context);

                    var downstreamHost = $"{serviceHostPort.Data.DownstreamHost}:{serviceHostPort.Data.DownstreamPort}";
                    logger.LogInformation($"Downstream IP Address: {downstreamHost}");

                    var channel = new Channel(downstreamHost, secureCredentials ?? ChannelCredentials.Insecure);
                    var client = new MethodDescriptorCaller(channel);

                    var requestObject = JsonConvert.DeserializeObject(requestData, methodDescriptor.InputType.ClrType);
                    var result = await client.InvokeAsync(methodDescriptor, context.HttpContext.GetRequestHeaders(), requestObject);
                    logger.LogInformation($"Request object data after gRPC called: {JsonConvert.SerializeObject(result)}");

                    var response = new OkResponse<GrpcHttpContent>(new GrpcHttpContent(JsonConvert.SerializeObject(result)));
                    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = response.Data
                    };

                    context.HttpContext.Response.ContentType = "application/json";
                    context.DownstreamResponse = new DownstreamResponse(httpResponseMessage);
                }
            }
        }
    }
}
