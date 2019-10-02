using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.Inventory.DataContracts.Dto.V1;

namespace VND.CoolStore.ShoppingCart.Domain.ProductCatalog
{
    public interface IInventoryGateway
    {
        Task<IEnumerable<InventoryDto>> GetAvailabilityInventories();
    }
}
