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
                if (httpContext.Request != null)
                {
                    httpContext.Request?.Headers.ToList().ForEach(h =>
                    {
                        if (h.Key.ToLowerInvariant() == "x-request-id" ||
                            h.Key.ToLowerInvariant() == "x-b3-traceid" ||
                            h.Key.ToLowerInvariant() == "x-b3-spanid" ||
                            h.Key.ToLowerInvariant() == "x-b3-parentspanid" ||
                            h.Key.ToLowerInvariant() == "x-b3-sampled" ||
                            h.Key.ToLowerInvariant() == "x-b3-flags" ||
                            h.Key.ToLowerInvariant() == "x-ot-span-context" ||
                            h.Key.ToLowerInvariant() == "authorization" ||
                            h.Key.ToLowerInvariant() == "x-role" ||
                            h.Key.ToLowerInvariant() == "version")
                        {
                            metadata.Add(h.Key, h.Value);
                        }
                    });
                }

                return await catchAction(metadata);
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception)
            {
                //throw new Exception($"{scope}: {ex.Message}");
                throw;
            }
        }
    }
}
