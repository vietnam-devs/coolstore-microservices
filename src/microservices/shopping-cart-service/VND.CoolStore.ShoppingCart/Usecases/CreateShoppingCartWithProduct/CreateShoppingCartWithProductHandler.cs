using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using VND.CoolStore.ShoppingCart.Data;
using VND.CoolStore.ShoppingCart.DataContracts.Api.V1;
using VND.CoolStore.ShoppingCart.DataContracts.Event.V1;
using VND.CoolStore.ShoppingCart.Domain;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.Usecases.CreateShoppingCartWithProduct
{
    public class CreateShoppingCartWithProductHandler : IRequestHandler<InsertItemToNewCartRequest, InsertItemToNewCartResponse>
    {
        private readonly IEfUnitOfWork<ShoppingCartDataContext> _unitOfWork;
        private readonly IProductCatalogService _productCatalogService;
        private readonly IPromoGateway _promoGateway;
        private readonly IShippingGateway _shippingGateway;

        public CreateShoppingCartWithProductHandler(
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

        public async Task<InsertItemToNewCartResponse> Handle(InsertItemToNewCartRequest request, CancellationToken cancellationToken)
        {
            var cartRepository = _unitOfWork.RepositoryAsync<Cart, Guid>();

            var cart = await Cart.Load(request.UserId.ConvertTo<Guid>())
                    .InsertItemToCart(request.ProductId.ConvertTo<Guid>(), request.Quantity)
                    .CalculateCartAsync(
                        TaxType.NoTax,
                        _productCatalogService,
                        _promoGateway,
                        _shippingGateway);

            await cartRepository.AddAsync(cart);
            var rowCount = await _unitOfWork.SaveChangesAsync(default);
            if (rowCount <= 0)
            {
                throw new Exception("Could not create data.");
            }

            return new InsertItemToNewCartResponse { Result = cart.ToDto(_productCatalogService) };
        }
    }

    // only for demo ability to subscribe inside the local service
    public class NotificationEnvelopeHandler : INotificationHandler<NotificationEnvelope>
    {
        public Task Handle(NotificationEnvelope notification, CancellationToken cancellationToken)
        {
            switch (notification.Event)
            {
                case ShoppingCartWithProductCreated @event:

                    // do something
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
