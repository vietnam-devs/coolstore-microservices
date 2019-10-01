using System.ComponentModel;
using MediatR;

namespace VND.CoolStore.Inventory.DataContracts.Api.V1
{
    [DefaultValue("DefaultReflection")]
    public static partial class InventoryApiReflection
    {
    }

    public partial class GetInventoriesRequest : IRequest<GetInventoriesResponse>
    {
    }

    public partial class GetInventoryRequest : IRequest<GetInventoryResponse>
    {
    }
}
