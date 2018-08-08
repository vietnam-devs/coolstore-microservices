using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore.CleanArch;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  public class CartRequestHandler : RequestHandlerBase<GetCartRequest, GetCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly NoTaxCaculator _priceCalculator;

    public CartRequestHandler(ICatalogGateway cgw, IUnitOfWorkAsync uow,
      IQueryRepositoryFactory qrf, NoTaxCaculator priceCalculator) : base(uow, qrf)
    {
      _catalogGateway = cgw;
      _priceCalculator = priceCalculator;
    }

    public override async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
      var cart = await QueryRepositoryFactory
        ?.QueryEfRepository<Domain.Cart>()
        ?.GetFullCart(request.CartId, false)
        ?.ToObservable()
        ?.SelectMany(c => c.InitCart(_catalogGateway, isPopulatePrice: true))
        ?.Select(c => _priceCalculator.Execute(c));

      return new GetCartResponse { Result = cart.ToCartDto() };
    }
  }
}
