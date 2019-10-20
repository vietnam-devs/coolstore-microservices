using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using VND.CoolStore.ShoppingCart.Data;
using VND.CoolStore.ShoppingCart.Data.Repositories;
using VND.CoolStore.ShoppingCart.DataContracts.Api.V1;
using VND.CoolStore.ShoppingCart.Domain;
using VND.CoolStore.ShoppingCart.Domain.Cart;
using VND.CoolStore.ShoppingCart.Domain.ProductCatalog;

namespace VND.CoolStore.ShoppingCart.Usecases.UpdateAmountOfProductInShoppingCart
{
    public class UpdateAmountOfProductInShoppingCartHandler : IRequestHandler<UpdateItemInCartRequest, UpdateItemInCartResponse>
    {
        private readonly IEfUnitOfWork<ShoppingCartDataContext> _unitOfWork;
        private readonly IProductCatalogService _productCatalogService;
        private readonly IPromoGateway _promoGateway;
        private readonly IShippingGateway _shippingGateway;
        private readonly IInventoryGateway _inventoryGateway;

        public UpdateAmountOfProductInShoppingCartHandler(
            IEfUnitOfWork<ShoppingCartDataContext> unitOfWork,
            IProductCatalogService productCatalogService,
            IPromoGateway promoGateway,
            IShippingGateway shippingGateway,
            IInventoryGateway inventoryGateway)
        {
            _unitOfWork = unitOfWork;
            _productCatalogService = productCatalogService;
            _promoGateway = promoGateway;
            _shippingGateway = shippingGateway;
            _inventoryGateway = inventoryGateway;
        }

        public async Task<UpdateItemInCartResponse> Handle(UpdateItemInCartRequest request, CancellationToken cancellationToken)
        {
            var cartRepository = _unitOfWork.RepositoryAsync<Cart, Guid>();
            var cartQueryRepository = _unitOfWork.QueryRepository<Cart, Guid>();

            var cart = await cartQueryRepository.GetFullCartAsync(request.CartId.ConvertTo<Guid>());
            var cartItem = cart.FindCartItem(request.ProductId.ConvertTo<Guid>());

            // if not exists then it should be a new item
            if (cartItem == null)
            {
                cart.InsertItemToCart(request.ProductId.ConvertTo<Guid>(), request.Quantity);
            }
            else
            {
                // otherwise is updating the current item in the cart
                cart.AccumulateCartItemQuantity(cartItem.Id, request.Quantity);
            }

            await cart.CalculateCartAsync(TaxType.NoTax, _productCatalogService, _promoGateway, _shippingGateway);

            await cartRepository.UpdateAsync(cart);
            var rowCount = await _unitOfWork.SaveChangesAsync(default);
            if (rowCount <= 0)
            {
                throw new Exception("Could not update data.");
            }

            return new UpdateItemInCartResponse { Result = await cart.ToDto(_productCatalogService, _inventoryGateway) };
        }
    }
}
