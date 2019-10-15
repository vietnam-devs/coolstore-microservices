using System.ComponentModel;
using MediatR;

namespace VND.CoolStore.ShoppingCart.DataContracts.Api.V1
{
    [DefaultValue("DefaultReflection")]
    public static partial class CartApiReflection
    {
    }

    public partial class GetCartRequest : IRequest<GetCartResponse>
    {
    }

    public partial class GetCartByUserIdRequest : IRequest<GetCartByUserIdResponse>
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
