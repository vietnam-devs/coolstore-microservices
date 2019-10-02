using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
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

        public async Task<GetInventoriesResponse> Handle(GetInventoriesRequest request, CancellationToken cancellationToken)
        {
            var result = new GetInventoriesResponse();
            await foreach (var inventory in GetInventoryAsyncStream())
            {
                result.Inventories.Add(new InventoryDto
                {
                    Id = inventory.Id.ToString(),
                    Location = inventory.Location,
                    Description = inventory.Description,
                    Website = inventory.Website
                });
            }

            return result;
        }

        private async IAsyncEnumerable<Domain.Inventory> GetInventoryAsyncStream()
        {
            using var conn = _unitOfWork.SqlConnectionFactory.GetOpenConnection();
            var reader = await conn.ExecuteReaderAsync(
                @"SELECT Id, Created, Updated, Version, Location, Description, Website
                    FROM InventoryDb.inventory.Inventories");

            while (reader.Read())
            {
                var id = reader["Id"].ToString().ConvertTo<Guid>();
                var location = reader["Location"].ToString();
                var description = reader["Description"].ToString();
                var website = reader["Website"].ToString();
                yield return Domain.Inventory.Of(id, location, description, website);
            }
        }
    }
}
