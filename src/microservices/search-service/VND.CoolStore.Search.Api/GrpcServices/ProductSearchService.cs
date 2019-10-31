using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using VND.CoolStore.Search.DataContracts.Api.V1;

namespace VND.CoolStore.Search.Api.GrpcServices
{
    public class ProductSearchService : ProductSearchApi.ProductSearchApiBase
    {
        private readonly IMediator _mediator;

        public ProductSearchService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<SearchProductResponse> SearchProduct(SearchProductRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }
    }
}
