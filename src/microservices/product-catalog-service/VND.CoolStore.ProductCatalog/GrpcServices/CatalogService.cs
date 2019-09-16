using System.Threading.Tasks;
using Grpc.Core;
using MediatR;

namespace VND.CoolStore.ProductCatalog.GrpcServices
{
    using VND.CoolStore.ProductCatalog.DataContracts.V1;
    using static VND.CoolStore.ProductCatalog.DataContracts.V1.Catalog;

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

        public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }
    }
}
