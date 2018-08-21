using System.Linq;
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

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  public class RequestHandler : TxRequestHandlerBase<UpdateItemInCartRequest, UpdateItemInCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly INoTaxPriceCalculator _priceCalculator;

    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf,
      ICatalogGateway catalogGateway, INoTaxPriceCalculator priceCalculator) : base(uow, qrf)
    {
      _catalogGateway = catalogGateway;
      _priceCalculator = priceCalculator;
    }

    public override async Task<UpdateItemInCartResponse> Handle(UpdateItemInCartRequest request,
      CancellationToken cancellationToken)
    {
      var cartCommander = UnitOfWork.Repository<Domain.Cart>();
      var cartItemCommander = UnitOfWork.Repository<CartItem>();
      var cartQuery = QueryRepositoryFactory.QueryEfRepository<Domain.Cart>();

      var isNewItem = false;
      var cart = await cartQuery
        .GetFullCartAsync(request.CartId)
        .ToObservable()
        .SelectMany(c => c.InitCart(_catalogGateway, isPopulatePrice: true));

      var item = cart.CartItems.FirstOrDefault(x => x.Product.ProductId == request.ProductId);

      // if not exists then it should be a new item
      if (item == null)
      {
        isNewItem = true;
        item = new CartItem()
        {
          Quantity = request.Quantity
        };
        item.LinkProduct(new Product(request.ProductId));
        cart.CartItems.Add(item);
      }
      else
      {
        // otherwise is updating the current item in the cart
        item.Quantity += request.Quantity;
      }

      cart = _priceCalculator.Execute(cart);

      // Todo: refactor to unit of work later
      if (!isNewItem)
        await cartItemCommander.UpdateAsync(item);
      else
        await cartItemCommander.AddAsync(item);

      await cartCommander.UpdateAsync(cart);
      await UnitOfWork.SaveChangesAsync(cancellationToken);

      return new UpdateItemInCartResponse {Result = cart.ToDto()};
    }
  }
}
