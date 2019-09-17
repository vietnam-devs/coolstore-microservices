using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VND.CoolStore.ProductCatalog.AppServices.UpdateProduct
{
    using CloudNativeKit.Infrastructure.Data.Dapper.Core;
    using CloudNativeKit.Utils.Extensions;
    using VND.CoolStore.ProductCatalog.DataContracts.V1;
    using VND.CoolStore.ProductCatalog.Domain;

    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, UpdateProductResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;

        public UpdateProductHandler(IDapperUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateProductResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var productQueryRepository = _unitOfWork.QueryRepository<Product, Guid>();
            var productRepository = _unitOfWork.RepositoryAsync<Product, Guid>();

            var existedProduct = await productQueryRepository.GetByIdAsync(request.ProductId.ConvertTo<Guid>());
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
