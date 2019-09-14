using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using VND.CoolStore.ProductCatalog.DataContracts.V1;
using VND.CoolStore.ProductCatalog.DataPersistence;

namespace VND.CoolStore.ProductCatalog.AppServices.GetDetailOfSpecificProduct
{
    public class GetDetailOfSpecificProductHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetDetailOfSpecificProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductAsync(request.ProductId.ConvertTo<Guid>());
            return new GetProductByIdResponse
            {
                Product = new CatalogProductDto
                {
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    Desc = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl
                }
            };
        }
    }
}
