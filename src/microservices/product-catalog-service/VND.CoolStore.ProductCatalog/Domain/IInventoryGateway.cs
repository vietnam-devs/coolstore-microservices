using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.Inventory.DataContracts.Dto.V1;

namespace VND.CoolStore.ProductCatalog.Domain
{
    public interface IInventoryGateway
    {
        Task<InventoryDto> GetInventoryAsync(Guid inventoryId);
        Task<IEnumerable<InventoryDto>> GetAvailabilityInventories();
    }
}
