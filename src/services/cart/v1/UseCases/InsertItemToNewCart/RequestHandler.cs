using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart
{
  public class RequestHandler : TxRequestHandlerBase<InsertItemToNewCartRequest, InsertItemToNewCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly IShippingGateway _shippingGateway;
    private readonly IPromoGateway _promoGateway;

    public RequestHandler(
      IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf,
      ICatalogGateway catalogGateway, IShippingGateway shippingGateway,
      IPromoGateway promoGateway) : base(uow, qrf)
    {
      _catalogGateway = catalogGateway;
      _shippingGateway = shippingGateway;
      _promoGateway = promoGateway;
    }

    public override async Task<InsertItemToNewCartResponse> Handle(InsertItemToNewCartRequest request, CancellationToken cancellationToken)
    {
      var cartCommander = CommandFactory.Repository<Domain.Cart>();

      var cart = await Domain.Cart.Load()
        .InsertItemToCart(request.ProductId, request.Quantity)
        .CalculateCartAsync(
          TaxType.NoTax,
          _catalogGateway,
          _promoGateway,
          _shippingGateway);

      await cartCommander.AddAsync(cart);

      return new InsertItemToNewCartResponse { Result = cart.ToDto() };
    }
  }
}
