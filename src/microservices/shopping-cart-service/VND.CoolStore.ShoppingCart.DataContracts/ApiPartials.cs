using MediatR;

namespace VND.CoolStore.ShoppingCart.DataContracts.V1
{
    public partial class GetCartRequest : IRequest<GetCartResponse>
    {
    }

    public partial class InsertItemToNewCartRequest : IRequest<InsertItemToNewCartResponse>
    {
    }

    public partial class UpdateItemInCartRequest : IRequest<UpdateItemInCartResponse>
    {
    }

    public partial class DeleteItemRequest : IRequest<DeleteItemResponse>
    {
    }

    public partial class CheckoutRequest : IRequest<CheckoutResponse>
    {
    }
}
