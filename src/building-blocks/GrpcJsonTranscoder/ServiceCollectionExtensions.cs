using GrpcJsonTranscoder.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GrpcJsonTranscoder
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcJsonTranscoder(this IServiceCollection services, Func<GrpcAssemblyResolver> addGrpcAssembly)
        {
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var svcProvider = scope.ServiceProvider;
                var config = svcProvider.GetRequiredService<IConfiguration>();
                services.Configure<GrpcMapperOptions>(config.GetSection("GrpcJsonTranscoder"));
                services.AddSingleton(resolver => addGrpcAssembly.Invoke());
                return services;
            }
        }
    }
}
