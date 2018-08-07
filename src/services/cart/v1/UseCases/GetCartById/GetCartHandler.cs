using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore.CleanArch;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  public class GetCartHandler : EventHandlerBase<GetCartRequest, GetCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly NoTaxCaculator _priceCalculator;

    public GetCartHandler(
      ICatalogGateway cgw,
      IUnitOfWorkAsync uow,
      IQueryRepositoryFactory qrf,
      NoTaxCaculator priceCalculator) : base(uow, qrf)
    {
      _catalogGateway = cgw;
      _priceCalculator = priceCalculator;
    }

    public override async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
      var cartQueryRepository = QueryRepositoryFactory.QueryEfRepository<Domain.Cart>();

      var cart = await cartQueryRepository.GetFullCart(request.CartId);
      cart = await cart.InitCart(_catalogGateway, isPopulatePrice: true);
      cart = _priceCalculator.Execute(cart);

      return new GetCartResponse { Result = cart.ToCartDto() };
    }
  }
}
