using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using VND.CoolStore.ProductCatalog.DataContracts.V1;
using static VND.CoolStore.ProductCatalog.DataContracts.V1.Catalog;

namespace VND.CoolStore.ProductCatalog.GrpcServices
{
    public class CatalogService : CatalogBase
    {
        private readonly IMediator _mediator;

        public CatalogService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<GetProductByIdResponse> GetProductById(GetProductByIdRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<CreateProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }
    }
}
