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
    private readonly INoTaxPriceCalculator _priceCalculator;

    public RequestHandler(
      IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf,
      ICatalogGateway catalogGateway, INoTaxPriceCalculator priceCalculator) : base(uow, qrf)
    {
      _catalogGateway = catalogGateway;
      _priceCalculator = priceCalculator;
    }

    public override async Task<InsertItemToNewCartResponse> Handle(InsertItemToNewCartRequest request, CancellationToken cancellationToken)
    {
      var cartCommander = UnitOfWork.Repository<Domain.Cart>();

      var cart = await Domain.Cart
        .Load()
        .InsertItemToCart(new CartItem
        {
          Product = new Product(request.ProductId),
          PromoSavings = 0.0D,
          Quantity = request.Quantity
        })
        .InitCart(_catalogGateway, isPopulatePrice: true)
        .ToObservable()
        .Select(c => _priceCalculator.Execute(c));

      await cartCommander.AddAsync(cart);
      await UnitOfWork.SaveChangesAsync(cancellationToken);

      return new InsertItemToNewCartResponse { Result = cart.ToDto() };
    }
  }
}
