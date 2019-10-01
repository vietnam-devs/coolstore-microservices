using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;

namespace VND.CoolStore.ProductCatalog.Api.GrpcServices
{
    [Authorize]
    public class CatalogService : CatalogApi.CatalogApiBase
    {
        private readonly IMediator _mediator;

        public CatalogService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
        {
            //var user = context.GetHttpContext().User;
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
