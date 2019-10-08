using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;
using VND.CoolStore.ProductCatalog.DataContracts.Dto.V1;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog.Usecases.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameHandler : IRequestHandler<GetProductsRequest, GetProductsResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;
        private readonly IInventoryGateway _inventoryGateway;

        public GetProductsByPriceAndNameHandler(IDapperUnitOfWork unitOfWork, IInventoryGateway inventoryGateway)
        {
            _unitOfWork = unitOfWork;
            _inventoryGateway = inventoryGateway;
        }

        public async Task<GetProductsResponse> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            var productRepository = _unitOfWork.QueryRepository<Product, Guid>();
            var queryable = productRepository.Queryable();
            var products = queryable
                .Skip(request.CurrentPage - 1)
                .Take(10)
                .Where(x => !x.IsDeleted && x.Price <= request.HighPrice)
                .ToList();

            var inventories = await _inventoryGateway.GetAvailabilityInventories();
            var response = new GetProductsResponse();
            response.Products
                .AddRange(products.Select(p =>
                {
                    var inventory = inventories.FirstOrDefault(x => x.Id.ConvertTo<Guid>() == p.InventoryId);
                    return new CatalogProductDto
                    {
                        Id = p.Id.ToString(),
                        Name = p.Name,
                        Desc = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        InventoryId = inventory?.Id,
                        InventoryLocation = inventory?.Location,
                        InventoryWebsite = inventory?.Website,
                        InventoryDescription = inventory?.Description
                    };
                }));

            return await Task.FromResult(response);
        }
    }
}
