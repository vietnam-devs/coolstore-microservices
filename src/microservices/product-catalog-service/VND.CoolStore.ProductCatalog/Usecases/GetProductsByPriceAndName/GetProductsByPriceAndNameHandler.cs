using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using VND.CoolStore.ProductCatalog.DataContracts.V1;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog.Usecases.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameHandler : IRequestHandler<GetProductsRequest, GetProductsResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;

        public GetProductsByPriceAndNameHandler(IDapperUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductsResponse> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            var productRepository = _unitOfWork.QueryRepository<Product, Guid>();
            var queryable = productRepository.Queryable();
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

            return await Task.FromResult(response);
        }
    }
}
