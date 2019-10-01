using FluentValidation;
using VND.CoolStore.Inventory.DataContracts.Api.V1;

namespace VND.CoolStore.Inventory.Usecases.GetInventory
{
    public class GetInventoryValidator : AbstractValidator<GetInventoryRequest>
    {
        public GetInventoryValidator()
        {
        }
    }
}
