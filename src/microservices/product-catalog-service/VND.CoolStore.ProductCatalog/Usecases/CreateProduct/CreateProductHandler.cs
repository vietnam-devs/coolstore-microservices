using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using VND.CoolStore.ProductCatalog.Domain;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;
using VND.CoolStore.ProductCatalog.DataContracts.Dto.V1;

namespace VND.CoolStore.ProductCatalog.Usecases.CreateProduct
{
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
