using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Query;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.V1;
using VND.CoolStore.ShoppingCart.DataPersistence;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.AppService.GetShoppingCartWithProducts
{
    public class GetShoppingCartWithProductsHandler : IRequestHandler<GetCartRequest, GetCartResponse>
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IQueryRepositoryFactory _queryRepositoryFactory;

        public GetShoppingCartWithProductsHandler(IQueryRepositoryFactory queryRepositoryFactory, IUnitOfWorkAsync unitOfWorkAsync)
        {
            _queryRepositoryFactory = queryRepositoryFactory;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
        {
            //todo: this is just for demo, will remove it soon
            var queryCartRepository = _queryRepositoryFactory.QueryRepository<Cart, Guid>();

            var existedCart = await queryCartRepository.GetByIdAsync<ShoppingCartDataContext, Cart, Guid>(request.CartId.ConvertTo<Guid>());
            if (existedCart != null)
                return new GetCartResponse { Result = new CartDto { Id = existedCart.Id.ToString() } };

            var cartRepository = _unitOfWorkAsync.RepositoryAsync<Cart, Guid>();
            var newCart = await cartRepository.AddAsync(Cart.Load(request.CartId.ConvertTo<Guid>()));
            newCart.AddEvent(new ShoppingCartWithProductCreated());
            await _unitOfWorkAsync.SaveChangesAsync(cancellationToken);

            return new GetCartResponse { Result = new CartDto { Id = newCart.Id.ToString() } };
        }
    }
}
