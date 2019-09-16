using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VND.CoolStore.ProductCatalog.AppServices.UpdateProduct
{
    using CloudNativeKit.Domain;
    using CloudNativeKit.Infrastructure.Data.Dapper.Repository;
    using CloudNativeKit.Utils.Extensions;
    using VND.CoolStore.ProductCatalog.DataContracts.V1;
    using VND.CoolStore.ProductCatalog.Domain;

    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, UpdateProductResponse>
    {
        private readonly IQueryRepositoryFactory _queryRepositoryFactory;

        public UpdateProductHandler(IQueryRepositoryFactory queryRepositoryFactory)
        {
            _queryRepositoryFactory = queryRepositoryFactory;
        }

        public async Task<UpdateProductResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var productRepository = _queryRepositoryFactory.QueryRepository<Product, Guid>() as IGenericRepository<Product, Guid>;
            var existedProduct = await productRepository.GetByIdAsync(request.ProductId.ConvertTo<Guid>());
            if (existedProduct == null)
            {
                throw new Exception("Could not get the record from the database.");
            }

            var updated = existedProduct.UpdateProduct(request);
            var product = await productRepository.UpdateAsync(updated);

            return new UpdateProductResponse
            {
                Product = new CatalogProductDto
                {
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    Desc = product.Description,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price
                }
            };
        }
    }
}
