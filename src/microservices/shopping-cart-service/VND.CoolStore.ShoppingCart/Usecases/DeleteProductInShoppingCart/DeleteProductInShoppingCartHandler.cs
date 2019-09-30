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

namespace VND.CoolStore.ShoppingCart.Usecases.DeleteProductInShoppingCart
{
    public class DeleteProductInShoppingCartHandler : IRequestHandler<DeleteItemRequest, DeleteItemResponse>
    {
        private readonly IEfUnitOfWork<ShoppingCartDataContext> _unitOfWork;
        private readonly IProductCatalogService _productCatalogService;
        private readonly IPromoGateway _promoGateway;
        private readonly IShippingGateway _shippingGateway;

        public DeleteProductInShoppingCartHandler(
            IEfUnitOfWork<ShoppingCartDataContext> unitOfWork,
            IProductCatalogService productCatalogService,
            IPromoGateway promoGateway,
            IShippingGateway shippingGateway)
        {
            _unitOfWork = unitOfWork;
            _productCatalogService = productCatalogService;
            _promoGateway = promoGateway;
            _shippingGateway = shippingGateway;
        }

        public async Task<DeleteItemResponse> Handle(DeleteItemRequest request, CancellationToken cancellationToken)
        {
            var cartRepository = _unitOfWork.RepositoryAsync<Cart, Guid>();
            var cartQueryRepository = _unitOfWork.QueryRepository<Cart, Guid>();

            var cart = await cartQueryRepository.GetFullCartAsync(request.CartId.ConvertTo<Guid>());
            var cartItem = cart.FindCartItem(request.ProductId.ConvertTo<Guid>());

            cart.RemoveCartItem(cartItem.Id);
            await cart.CalculateCartAsync(TaxType.NoTax, _productCatalogService, _promoGateway, _shippingGateway);

            await cartRepository.UpdateAsync(cart);
            var rowCount = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (rowCount <= 0)
            {
                throw new Exception("Could not delete data.");
            }

            return new DeleteItemResponse { ProductId = cartItem.Product.ProductId.ToString() };

        }
    }
}
