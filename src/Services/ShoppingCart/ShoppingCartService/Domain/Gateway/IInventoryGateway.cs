using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartService.Domain.Dto;
using ShoppingCartService.Domain.Model;

namespace ShoppingCartService.Domain.Gateway
{
    public interface IInventoryGateway
    {
        Task<IEnumerable<InventoryDto>> GetAvailabilityInventories();
    }
}
