using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;
using VND.CoolStore.ProductCatalog.DataContracts.Dto.V1;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog.Usecases.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;

        public DeleteProductHandler(IDapperUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteProductResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var productQueryRepository = _unitOfWork.QueryRepository<Product, Guid>();
            var productRepository = _unitOfWork.RepositoryAsync<Product, Guid>();

            var existedProduct = await productQueryRepository.GetByIdAsync(request.ProductId.ConvertTo<Guid>());
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
