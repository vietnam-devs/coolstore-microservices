using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VND.CoolStore.ProductCatalog.AppServices.DeleteProduct
{
    using CloudNativeKit.Domain;
    using CloudNativeKit.Infrastructure.Data.Dapper.Repository;
    using CloudNativeKit.Utils.Extensions;
    using VND.CoolStore.ProductCatalog.DataContracts.V1;
    using VND.CoolStore.ProductCatalog.Domain;

    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
    {
        private readonly IQueryRepositoryFactory _queryRepositoryFactory;

        public DeleteProductHandler(IQueryRepositoryFactory queryRepositoryFactory)
        {
            _queryRepositoryFactory = queryRepositoryFactory;
        }

        public async Task<DeleteProductResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var productRepository = _queryRepositoryFactory.QueryRepository<Product, Guid>() as IGenericRepository<Product, Guid>;
            var existedProduct = await productRepository.GetByIdAsync(request.ProductId.ConvertTo<Guid>());
            if (existedProduct == null)
            {
                throw new Exception("Could not get the record from the database.");
            }

            await productRepository.DeleteAsync(existedProduct);

            return new DeleteProductResponse
            {
                Product = new CatalogProductDto
                {
                    Id = existedProduct.Id.ToString(),
                    Name = existedProduct.Name,
                    Desc = existedProduct.Description,
                    ImageUrl = existedProduct.ImageUrl,
                    Price = existedProduct.Price
                }
            };
        }
    }
}
