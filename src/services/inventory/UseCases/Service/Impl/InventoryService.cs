using System;
using System.Linq;
using VND.FW.Infrastructure.EfCore.Repository;

namespace VND.CoolStore.Services.Inventory.UseCases.Service.Impl
{
  public class InventoryService : IInventoryService
  {
    private IEfQueryRepository<Domain.Inventory> _repo;
    public InventoryService(IEfQueryRepository<Domain.Inventory> repo)
    {
      _repo = repo;
    }

    public Domain.Inventory GetInventory(Guid itemId)
    {
      return _repo.Queryable().FirstOrDefault(x => x.Id == itemId);
    }
  }
}
