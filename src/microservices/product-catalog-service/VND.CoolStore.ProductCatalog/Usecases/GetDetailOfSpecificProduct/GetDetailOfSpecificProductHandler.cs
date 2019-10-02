using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;
using VND.CoolStore.ProductCatalog.DataContracts.Dto.V1;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog.Usecases.GetDetailOfSpecificProduct
{
    public class GetDetailOfSpecificProductHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;
        private readonly ILogger<GetDetailOfSpecificProductHandler> _logger;

        public GetDetailOfSpecificProductHandler(IDapperUnitOfWork unitOfWork, ILogger<GetDetailOfSpecificProductHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var productRepository = _unitOfWork.QueryRepository<Product, Guid>();
            var existedProducts = await productRepository.GetByConditionAsync(new { Id = request.ProductId.ConvertTo<Guid>(), IsDeleted = false });
            var existedProduct = existedProducts.FirstOrDefault(); // only get one
            if (existedProduct == null)
            {
                existedProduct = Product.Of(new CreateProductRequest());
                _logger.LogInformation($"Could not find record with id #{request.ProductId} in the database.");
            }

            return new GetProductByIdResponse
            {
                Product = new CatalogProductDto
                {
                    Id = existedProduct.Id.ToString(),
                    Name = existedProduct.Name,
                    Desc = existedProduct.Description,
                    Price = existedProduct.Price,
                    ImageUrl = existedProduct.ImageUrl
                }
            };
        }
    }
}
