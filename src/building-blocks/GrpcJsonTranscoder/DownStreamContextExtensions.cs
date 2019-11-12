using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcJsonTranscoder.Grpc;
using GrpcJsonTranscoder.Internal.Grpc;
using GrpcJsonTranscoder.Internal.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ocelot.LoadBalancer.LoadBalancers;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responses;

namespace GrpcJsonTranscoder
{
    public static class DownStreamContextExtensions
    {
        public static async Task HandleGrpcRequestAsync(this DownstreamContext context, Func<Task> next, IEnumerable<Interceptor> interceptors = null, SslCredentials secureCredentials = null)
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
                    var upstreamHeaders = new Dictionary<string, string>
                            {
                                { "x-grpc-route-data", JsonConvert.SerializeObject(context.TemplatePlaceholderNameAndValues.Select(x => new {x.Name, x.Value})) },
                                { "x-grpc-body-data", await context.DownstreamRequest.Content.ReadAsStringAsync() }
                            };

                    logger.LogInformation($"Upstream request method is {context.HttpContext.Request.Method}");
                    logger.LogInformation($"Upstream header data for x-grpc-route-data is {upstreamHeaders["x-grpc-route-data"]}");
                    logger.LogInformation($"Upstream header data for x-grpc-body-data is {upstreamHeaders["x-grpc-body-data"]}");
                    var requestObject = context.HttpContext.ParseRequestData(upstreamHeaders);
                    var requestJsonData = JsonConvert.SerializeObject(requestObject);
                    logger.LogInformation($"Request object data is {requestJsonData}");

                    var loadBalancerFactory = context.HttpContext.RequestServices.GetService<ILoadBalancerFactory>();
                    var loadBalancerResponse = await loadBalancerFactory.Get(context.DownstreamReRoute, context.Configuration.ServiceProviderConfiguration);
                    var serviceHostPort = await loadBalancerResponse.Data.Lease(context);

                    var downstreamHost = $"{serviceHostPort.Data.DownstreamHost}:{serviceHostPort.Data.DownstreamPort}";
                    logger.LogInformation($"Downstream IP Address is {downstreamHost}");

                    var channel = new Channel(downstreamHost, secureCredentials ?? ChannelCredentials.Insecure);

                    MethodDescriptorCaller client = null;

                    if (interceptors != null && interceptors.Count() > 0)
                    {
                        CallInvoker callInvoker = null;

                        foreach (var inteceptor in interceptors)
                        {
                            callInvoker = channel.Intercept(inteceptor);
                        }

                        client = new MethodDescriptorCaller(callInvoker);
                    }
                    else
                    {
                        client = new MethodDescriptorCaller(channel);
                    }
                    
                    var concreteObject = JsonConvert.DeserializeObject(requestJsonData, methodDescriptor.InputType.ClrType);
                    var result = await client.InvokeAsync(methodDescriptor, context.HttpContext.GetRequestHeaders(), concreteObject);
                    logger.LogDebug($"gRPC response called with {JsonConvert.SerializeObject(result)}");

                    var jsonSerializer = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                    var response = new OkResponse<GrpcHttpContent>(new GrpcHttpContent(JsonConvert.SerializeObject(result, jsonSerializer)));

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
