using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Extensions;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.Fw.Infrastructure.AspNetCore.CleanArch;
using VND.Fw.Infrastructure.EfCore.Extensions;

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
      var cartRepo = QueryRepositoryFactory?.QueryEfRepository<Domain.Cart>();

      var cartInfo = await cartRepo.GetFullCart(request.CartId, false);
      cartInfo = await cartInfo.InitCart(_catalogGateway, isPopulatePrice: true);
      cartInfo = _priceCalculator.Execute(cartInfo);

      return new GetCartResponse { Result = cartInfo.ToCartDto() };
    }
  }
}
