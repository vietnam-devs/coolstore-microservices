using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
using Dapper;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VND.CoolStore.Inventory.DataContracts.Api.V1;

namespace VND.CoolStore.Inventory.Api.GrpcServices
{
    public class InventoryService : InventoryApi.InventoryApiBase
    {
        private readonly IMediator _mediator;
        private readonly IDapperUnitOfWork _unitOfWork;

        public InventoryService(IMediator mediator, IDapperUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public override async Task<GetInventoriesResponse> GetInventories(GetInventoriesRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<GetInventoryResponse> GetInventory(GetInventoryRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task GetInventoryAsyncStream(GetInventoryStreamRequest request, IServerStreamWriter<GetInventoryStreamResponse> responseStream, ServerCallContext context)
        {
            await foreach (var inventory in GetInventoryAsyncStream())
            {
                await responseStream.WriteAsync(new GetInventoryStreamResponse
                {
                    Id = inventory.Id.ToString(),
                    Location = inventory.Location,
                    Description = inventory.Description,
                    Website = inventory.Website
                });
            }
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
