using FluentValidation;
using VND.CoolStore.Inventory.DataContracts.Api.V1;

namespace VND.CoolStore.Inventory.Usecases.GetAvailabilityInventories
{
    public class GetAvailabilityInventoriesValidator : AbstractValidator<GetInventoriesRequest>
    {
        public GetAvailabilityInventoriesValidator()
        {
        }
    }
}
