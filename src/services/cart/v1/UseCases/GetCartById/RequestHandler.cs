using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Extensions;
using VND.CoolStore.Services.Cart.v1.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  public class RequestHandler : RequestHandlerBase<GetCartRequest, GetCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly IShippingGateway _shippingGateway;
    private readonly IPromoGateway _promoGateway;

    public RequestHandler(ICatalogGateway cgw, IQueryRepositoryFactory qrf,
      IShippingGateway shippingGateway, IPromoGateway promoGateway)
      : base(qrf)
    {
      _catalogGateway = cgw;
      _shippingGateway = shippingGateway;
      _promoGateway = promoGateway;
    }

    public override async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
      var cartQuery = QueryFactory.QueryEfRepository<Domain.Cart>();

      var cart = await cartQuery.GetFullCartAsync(request.CartId, false)
        .ToObservable()
        .SelectMany(async c =>
          await c.CalculateCartAsync(TaxType.NoTax, _catalogGateway, _promoGateway, _shippingGateway));

      return new GetCartResponse { Result = cart.ToDto() };
    }
  }
}
