using System;
using MediatR;
using VND.CoolStore.Services.Inventory.Dtos;

namespace VND.CoolStore.Services.Inventory.v1.UseCases.GetInventory
{
  public class GetInventoryRequest : IRequest<GetInventoryResponse>
  {
    public Guid InventoryId { get; set; }
  }

  public class GetInventoryResponse
  {
    public InventoryDto Result { get; set; }
  }
}
