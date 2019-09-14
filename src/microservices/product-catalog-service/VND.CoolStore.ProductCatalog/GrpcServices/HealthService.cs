using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Health.V1;

namespace VND.CoolStore.ProductCatalog.GrpcServices
{
    public class HealthService : Health.HealthBase
    {
        public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            // should check database, bus or redis connection
            // ...

            return Task.FromResult(new HealthCheckResponse
            {
                Status = HealthCheckResponse.Types.ServingStatus.Serving
            });
        }
    }
}
