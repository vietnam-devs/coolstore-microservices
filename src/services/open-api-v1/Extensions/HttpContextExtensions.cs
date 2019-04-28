using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VND.CoolStore.Services.OpenApiV1.v1.Grpc
{
    public static class HttpContextExtensions
    {
        public static async ValueTask<IActionResult> EnrichGrpcWithHttpContext(
            this HttpContext httpContext, string scope,
            Func<Metadata, ValueTask<IActionResult>> catchAction)
        {
            try
            {
                var metadata = new Metadata();
                if (httpContext.Request != null
                    && httpContext.Request.Headers.Any(
                        h => h.Key.ToLower().Contains("authorization")))
                {
                    metadata.Add("authorization", httpContext.Request?.Headers["authorization"]);
                }

                // add supporting for OpenTracing standard
                httpContext.Request?.Headers.ToList().ForEach(h =>
                {
                    if (h.Key == "x-request-id" ||
                                h.Key == "x-b3-traceid" ||
                                h.Key == "x-b3-spanid" ||
                                h.Key == "x-b3-parentspanid" ||
                                h.Key == "x-b3-sampled" ||
                                h.Key == "x-b3-flags" ||
                                h.Key == "x-ot-span-context")
                    {
                        metadata.Add(h.Key, h.Value);
                    }
                });

                return await catchAction(metadata);
            }
            catch (RpcException ex)
            {
                throw new Exception($"{scope}: {ex.Message}");
            }
        }
    }
}
