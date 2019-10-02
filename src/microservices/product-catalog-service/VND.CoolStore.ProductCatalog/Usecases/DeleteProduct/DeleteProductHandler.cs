using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;
using VND.CoolStore.ProductCatalog.DataContracts.Dto.V1;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog.Usecases.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(IDapperUnitOfWork unitOfWork, ILogger<DeleteProductHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<DeleteProductResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var productQueryRepository = _unitOfWork.QueryRepository<Product, Guid>();
            var productRepository = _unitOfWork.RepositoryAsync<Product, Guid>();

            var existedProduct = await productQueryRepository.GetByIdAsync(request.ProductId.ConvertTo<Guid>());
            if (existedProduct != null)
            {
                existedProduct.MarkAsDeleted();
                await productRepository.UpdateAsync(existedProduct);
            }
            else
            {
                existedProduct = Product.Of(new CreateProductRequest());
            }

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
