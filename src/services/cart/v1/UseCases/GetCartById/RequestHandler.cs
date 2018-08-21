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
using VND.CoolStore.Services.Cart.v1.Services;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  public class RequestHandler : RequestHandlerBase<GetCartRequest, GetCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly INoTaxPriceCalculator _priceCalculator;

    public RequestHandler(ICatalogGateway cgw, IQueryRepositoryFactory qrf, INoTaxPriceCalculator priceCalculator)
      : base(qrf)
    {
      _catalogGateway = cgw;
      _priceCalculator = priceCalculator;
    }

    public override async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
      var cartQuery = QueryRepositoryFactory.QueryEfRepository<Domain.Cart>();

      var cartInfo = await cartQuery.GetFullCartAsync(request.CartId, false);
      cartInfo = await cartInfo
        .InitCart(_catalogGateway, isPopulatePrice: true)
        .ToObservable()
        .Select(x => _priceCalculator.Execute(x));

      return new GetCartResponse { Result = cartInfo.ToDto() };
    }
  }
}
