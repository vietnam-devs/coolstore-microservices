using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using CloudNativeKit.Utils.Extensions;
using VND.CoolStore.ShoppingCart.Data;
using VND.CoolStore.ShoppingCart.DataContracts.V1;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.Usecases.GetShoppingCartWithProducts
{
    public class GetShoppingCartWithProductsHandler : IRequestHandler<GetCartRequest, GetCartResponse>
    {
        private readonly IEfUnitOfWork<ShoppingCartDataContext> _unitOfWork;

        public GetShoppingCartWithProductsHandler(IEfUnitOfWork<ShoppingCartDataContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
        {
            //todo: this is just for demo, will remove it soon
            var queryCartRepository = _unitOfWork.QueryRepository<Cart, Guid>();

            var existedCart = await queryCartRepository.GetByIdAsync<ShoppingCartDataContext, Cart, Guid>(request.CartId.ConvertTo<Guid>());
            if (existedCart != null)
                return new GetCartResponse { Result = new CartDto { Id = existedCart.Id.ToString() } };

            var cartRepository = _unitOfWork.RepositoryAsync<Cart, Guid>();
            var newCart = await cartRepository.AddAsync(Cart.Load(request.CartId.ConvertTo<Guid>()));
            newCart.AddEvent(new ShoppingCartWithProductCreated());
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new GetCartResponse { Result = new CartDto { Id = newCart.Id.ToString() } };
        }
    }
}
