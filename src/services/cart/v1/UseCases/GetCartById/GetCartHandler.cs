using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.Infrastructure.Services;
using VND.CoolStore.Services.Cart.UseCases.v1.Services;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Repository;
using VND.FW.Infrastructure.EfCore.Service;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  public class GetCartHandler : CartServiceBase,
    IRequestHandler<GetCartRequest, GetCartResponse>,
    IQueryService
  {
    private readonly NoTaxCaculator _priceCalculator;

    public GetCartHandler(
      ICatalogGateway catalogGateway,
      IQueryRepositoryFactory queryRepositoryFactory,
      NoTaxCaculator priceCalculator)
      : base(catalogGateway)
    {
      QueryRepositoryFactory = queryRepositoryFactory;
      _priceCalculator = priceCalculator;
    }

    public PriceCalculatorContext PriceCalculatorContext { get; set; }
    public IQueryRepositoryFactory QueryRepositoryFactory { get; }

    public override IEfQueryRepository<Domain.Cart> GetQueryRepository()
    {
      return QueryRepositoryFactory.QueryRepository<Domain.Cart>() as IEfQueryRepository<Domain.Cart>;
    }

    public async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
      var cart = await GetCart(request.CartId);
      cart = await InitCart(cart, populatePrice: true);
      cart = _priceCalculator.Execute(cart);

      return new GetCartResponse
      {
        Result = GetCartByIdResponse(cart)
      };
    }
  }
}
