using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using VND.CoolStore.Inventory.DataContracts.Api.V1;
using VND.CoolStore.Inventory.DataContracts.Dto.V1;
using VND.CoolStore.ProductCatalog.Domain;
using static VND.CoolStore.Inventory.DataContracts.Api.V1.InventoryApi;

namespace VND.CoolStore.ProductCatalog.Gateways
{
    public class InventoryGateway : IInventoryGateway
    {
        private readonly InventoryApiClient _client;

        public InventoryGateway(IConfiguration config)
        {
            _client = new InventoryApiClient(new Channel(config["Grpc:InventoryHost"], ChannelCredentials.Insecure));
        }

        public async Task<InventoryDto> GetInventoryAsync(Guid inventoryId)
        {
            var inventoryResponse = await _client.GetInventoryAsync(new GetInventoryRequest { Id = inventoryId.ToString() });
            return new InventoryDto
            {
                Id = inventoryResponse.Result?.Id,
                Location = inventoryResponse.Result?.Location,
                Website = inventoryResponse.Result?.Website,
                Description = inventoryResponse.Result?.Description
            };
        }

        public async Task<IEnumerable<InventoryDto>> GetAvailabilityInventories()
        {
            using var stream = _client.GetInventoryAsyncStream(new GetInventoryStreamRequest());
            var result = new List<InventoryDto>();
            await foreach (var response in stream.ResponseStream.ReadAllAsync())
            {
                result.Add(new InventoryDto
                {
                    Id = response.Id,
                    Location = response.Location,
                    Description = response.Description,
                    Website = response.Website
                });
            }

            return result;
        }
    }
}
