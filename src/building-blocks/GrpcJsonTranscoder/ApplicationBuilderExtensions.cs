using GrpcJsonTranscoder.Internal.Middleware;
using Microsoft.AspNetCore.Builder;

namespace GrpcJsonTranscoder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGrpcJsonTranscoder(this IApplicationBuilder app)
        {
            app.UseMiddleware<GrpcJsonTranscoderMiddleware>();
            return app;
        }
    }
}
