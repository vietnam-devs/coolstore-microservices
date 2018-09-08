using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public class RequestHandler : TxRequestHandlerBase<DeleteItemRequest, DeleteItemResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly IShippingGateway _shippingGateway;
    private readonly IPromoGateway _promoGateway;

    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qf,
      ICatalogGateway cgw, IShippingGateway shippingGateway, IPromoGateway promoGateway)
      : base(uow, qf)
    {
      _catalogGateway = cgw;
      _shippingGateway = shippingGateway;
      _promoGateway = promoGateway;
    }

    public override async Task<DeleteItemResponse> Handle(DeleteItemRequest request, CancellationToken cancellationToken)
    {
      var cartCommander = CommandFactory.Repository<Domain.Cart>();
      var cartQuery = QueryFactory.QueryEfRepository<Domain.Cart>();

      var cart = await cartQuery.GetFullCartAsync(request.CartId);
      var cartItem = cart.FindCartItem(request.ProductId);

      cart.RemoveCartItem(cartItem.Id);
      await cart.CalculateCartAsync(TaxType.NoTax, _catalogGateway, _promoGateway, _shippingGateway);
      await cartCommander.UpdateAsync(cart);

      return new DeleteItemResponse { ProductId = cartItem.Product.ProductId };
    }
  }
}
