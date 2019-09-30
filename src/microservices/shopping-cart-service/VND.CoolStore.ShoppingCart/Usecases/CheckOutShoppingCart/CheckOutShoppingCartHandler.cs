using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using VND.CoolStore.ShoppingCart.Data;
using VND.CoolStore.ShoppingCart.Data.Repositories;
using VND.CoolStore.ShoppingCart.DataContracts.Api.V1;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.Usecases.CheckOutShoppingCart
{
    public class CheckOutShoppingCartHandler : IRequestHandler<CheckoutRequest, CheckoutResponse>
    {
        private readonly IEfUnitOfWork<ShoppingCartDataContext> _unitOfWork;

        public CheckOutShoppingCartHandler(IEfUnitOfWork<ShoppingCartDataContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancellationToken)
        {
            var cartRepository = _unitOfWork.RepositoryAsync<Cart, Guid>();
            var cartQueryRepository = _unitOfWork.QueryRepository<Cart, Guid>();

            var cart = await cartQueryRepository.GetFullCartAsync(request.CartId.ConvertTo<Guid>());

            var checkoutCart = await cartRepository.UpdateAsync(cart.Checkout());
            var rowCount = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (rowCount <= 0)
            {
                throw new Exception("Could not checkout.");
            }

            return new CheckoutResponse
            {
                IsSucceed = checkoutCart != null
            };
        }
    }
}
