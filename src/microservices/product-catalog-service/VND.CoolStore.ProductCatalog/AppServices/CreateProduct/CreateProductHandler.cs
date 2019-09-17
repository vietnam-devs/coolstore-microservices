using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VND.CoolStore.ProductCatalog.AppServices.CreateProduct
{
    using CloudNativeKit.Infrastructure.Data.Dapper.Core;
    using VND.CoolStore.ProductCatalog.DataContracts.V1;
    using VND.CoolStore.ProductCatalog.Domain;

    public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;

        public CreateProductHandler(IDapperUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var productRepository = _unitOfWork.RepositoryAsync<Product, Guid>();

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
