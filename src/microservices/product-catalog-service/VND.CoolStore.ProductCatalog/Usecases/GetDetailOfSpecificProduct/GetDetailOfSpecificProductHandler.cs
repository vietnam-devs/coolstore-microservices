using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
using VND.CoolStore.ProductCatalog.DataContracts.V1;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog.Usecases.GetDetailOfSpecificProduct
{
    public class GetDetailOfSpecificProductHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;

        public GetDetailOfSpecificProductHandler(IDapperUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var productRepository = _unitOfWork.QueryRepository<Product, Guid>();
            var existedProduct = await productRepository.GetByIdAsync(request.ProductId.ConvertTo<Guid>());
            if (existedProduct == null)
            {
                throw new Exception("Could not get the record from the database.");
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
