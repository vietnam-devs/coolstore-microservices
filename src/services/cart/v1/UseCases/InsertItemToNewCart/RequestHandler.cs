using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Extensions;
using VND.CoolStore.Services.Cart.v1.Services;

namespace VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart
{
  public class RequestHandler : TxRequestHandlerBase<InsertItemToNewCartRequest, InsertItemToNewCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly NoTaxCaculator _priceCalculator;

    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf,
      ICatalogGateway catalogGateway, NoTaxCaculator priceCalculator) : base(uow, qrf)
    {
      _catalogGateway = catalogGateway;
      _priceCalculator = priceCalculator;
    }

    public override async Task<InsertItemToNewCartResponse> TxHandle(InsertItemToNewCartRequest request, CancellationToken cancellationToken)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();

      var cart = await Domain.Cart
        .Load()
        ?.InsertItemToCart(new CartItem
        {
          Product = new Product(request.ProductId),
          PromoSavings = 0.0D,
          Quantity = request.Quantity
        })
        ?.InitCart(_catalogGateway, isPopulatePrice: true)
        ?.ToObservable()
        ?.Select(c => _priceCalculator.Execute(c));

      await cartRepository.AddAsync(cart);

      return new InsertItemToNewCartResponse { Result = cart.ToDto() };
    }
  }
}
