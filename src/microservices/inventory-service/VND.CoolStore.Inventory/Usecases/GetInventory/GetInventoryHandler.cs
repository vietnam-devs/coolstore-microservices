using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using VND.CoolStore.Inventory.DataContracts.Api.V1;
using VND.CoolStore.Inventory.DataContracts.Dto.V1;

namespace VND.CoolStore.Inventory.Usecases.GetInventory
{
    public class GetInventoryHandler : IRequestHandler<GetInventoryRequest, GetInventoryResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;

        public GetInventoryHandler(IDapperUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetInventoryResponse> Handle(GetInventoryRequest request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.Inventory, Guid>();
            var inventory = await queryRepository.GetByIdAsync(request.Id.ConvertTo<Guid>());
            if (inventory == null)
            {
                throw new Exception("Could not get the record from the database.");
            }

            return new GetInventoryResponse
            {
                Result = new InventoryDto
                {
                    Id = inventory.Id.ToString(),
                    Location = inventory.Location,
                    Description = inventory.Description,
                    Website = inventory.Website
                }
            };
        }
    }
}
