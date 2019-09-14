using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.ProductCatalog.DataContracts.V1;
using VND.CoolStore.ProductCatalog.DataPersistence;

namespace VND.CoolStore.ProductCatalog.AppServices.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameHandler : IRequestHandler<GetProductsRequest, GetProductsResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByPriceAndNameHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetProductsResponse> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductsAsync(request.CurrentPage, request.HighPrice);
            var response = new GetProductsResponse();
            response.Products
                .AddRange(products.Select(p => new CatalogProductDto
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Desc = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                })
                .ToList());

            return response;
        }
    }
}
