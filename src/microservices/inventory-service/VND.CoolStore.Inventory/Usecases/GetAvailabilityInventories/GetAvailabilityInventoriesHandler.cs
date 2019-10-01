using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using Dapper;
using MediatR;
using VND.CoolStore.Inventory.DataContracts.Api.V1;
using VND.CoolStore.Inventory.DataContracts.Dto.V1;

namespace VND.CoolStore.Inventory.Usecases.GetAvailabilityInventories
{
    public class GetAvailabilityInventoriesHandler : IRequestHandler<GetInventoriesRequest, GetInventoriesResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;

        public GetAvailabilityInventoriesHandler(IDapperUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<GetInventoriesResponse> Handle(GetInventoriesRequest request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.Inventory, Guid>();
            var inventories = queryRepository.Queryable().AsList();

            var result = new GetInventoriesResponse();
            result.Inventories.AddRange(
                inventories.Select(x => new InventoryDto
                {
                    Id = x.Id.ToString(),
                    Location = x.Location,
                    Description = x.Description,
                    Website = x.Website
                }));

            return Task.FromResult(result);
        }
    }
}
