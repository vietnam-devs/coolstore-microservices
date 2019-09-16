using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VND.CoolStore.ProductCatalog.AppServices.CreateProduct
{
    using CloudNativeKit.Domain;
    using CloudNativeKit.Infrastructure.Data.Dapper.Repository;
    using VND.CoolStore.ProductCatalog.DataContracts.V1;
    using VND.CoolStore.ProductCatalog.Domain;

    public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        private readonly IQueryRepositoryFactory _queryRepositoryFactory;

        public CreateProductHandler(IQueryRepositoryFactory queryRepositoryFactory)
        {
            _queryRepositoryFactory = queryRepositoryFactory;
        }

        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var productRepository = _queryRepositoryFactory.QueryRepository<Product, Guid>() as IGenericRepository<Product, Guid>;

            var product = Product.Of(request);
            var created = await productRepository.AddAsync(product);

            return new CreateProductResponse
            {
                Product = new CatalogProductDto
                {
                    Id = created.Id.ToString(),
                    Name = created.Name,
                    Desc = created.Description,
                    ImageUrl = created.ImageUrl,
                    Price = created.Price
                }
            };
        }
    }
}
