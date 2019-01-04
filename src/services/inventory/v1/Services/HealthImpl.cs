using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Health.V1;

namespace VND.CoolStore.Services.Inventory.v1.Services
{
    public class HealthImpl : Health.HealthBase
    {
        public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            Console.WriteLine("Checking ReviewService Health...");

            return Task.FromResult(new HealthCheckResponse
            {
                Status = HealthCheckResponse.Types.ServingStatus.Serving
            });
        }
    }
}
