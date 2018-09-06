using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Extensions;
using VND.CoolStore.Services.Cart.v1.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  public class RequestHandler : TxRequestHandlerBase<UpdateItemInCartRequest, UpdateItemInCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly IShippingGateway _shippingGateway;
    private readonly IPromoGateway _promoGateway;

    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf,
      ICatalogGateway catalogGateway, IShippingGateway shippingGateway,
      IPromoGateway promoGateway) : base(uow, qrf)
    {
      _catalogGateway = catalogGateway;
      _shippingGateway = shippingGateway;
      _promoGateway = promoGateway;
    }

    public override async Task<UpdateItemInCartResponse> Handle(UpdateItemInCartRequest request,
      CancellationToken cancellationToken)
    {
      var cartCommander = UnitOfWork.Repository<Domain.Cart>();
      var cartQuery = QueryFactory.QueryEfRepository<Domain.Cart>();

      var cart = await cartQuery
        .GetFullCartAsync(request.CartId);

      var cartItem = cart.FindCartItem(request.ProductId);

      // if not exists then it should be a new item
      if (cartItem == null)
      {
        cart.InsertItemToCart(request.ProductId, request.Quantity);
      }
      else
      {
        // otherwise is updating the current item in the cart
        cart.AccumulateCartItemQuantity(cartItem.Id, request.Quantity);
      }

      cart = await cart.CalculateCartAsync(TaxType.NoTax, _catalogGateway, _promoGateway, _shippingGateway);

      await cartCommander.UpdateAsync(cart);
      await UnitOfWork.SaveChangesAsync(cancellationToken);

      return new UpdateItemInCartResponse {Result = cart.ToDto()};
    }
  }
}
