using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VND.CoolStore.ProductCatalog.AppServices.GetProductsByPriceAndName
{
    using CloudNativeKit.Domain;
    using CloudNativeKit.Infrastructure.Data.Dapper.Repository;
    using VND.CoolStore.ProductCatalog.DataContracts.V1;
    using VND.CoolStore.ProductCatalog.Domain;

    public class GetProductsByPriceAndNameHandler : IRequestHandler<GetProductsRequest, GetProductsResponse>
    {
        private readonly IQueryRepositoryFactory _queryRepositoryFactory;

        public GetProductsByPriceAndNameHandler(IQueryRepositoryFactory queryRepositoryFactory)
        {
            _queryRepositoryFactory = queryRepositoryFactory;
        }

        public async Task<GetProductsResponse> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            var productRepository = _queryRepositoryFactory.QueryRepository<Product, Guid>() as IGenericRepository<Product, Guid>;
            var queryable = await productRepository.QueryableAsync();
            var products = queryable
                .Skip(request.CurrentPage - 1)
                .Take(10)
                .Where(x => x.Price <= request.HighPrice);

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
