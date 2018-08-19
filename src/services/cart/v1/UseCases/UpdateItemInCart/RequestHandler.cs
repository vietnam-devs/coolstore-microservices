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
    private readonly NoTaxCaculator _priceCalculator;

    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf,
      ICatalogGateway catalogGateway, NoTaxCaculator priceCalculator) : base(uow, qrf)
    {
      _catalogGateway = catalogGateway;
      _priceCalculator = priceCalculator;
    }

    public override async Task<UpdateItemInCartResponse> Handle(UpdateItemInCartRequest request,
      CancellationToken cancellationToken)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cartItemRepository = UnitOfWork.Repository<CartItem>();

      var isNewItem = false;
      var cart = await QueryRepositoryFactory
        ?.QueryEfRepository<Domain.Cart>()
        ?.GetFullCart(request.CartId)
        ?.ToObservable()
        ?.SelectMany(c => c.InitCart(_catalogGateway, isPopulatePrice: true));

      var item = cart.CartItems.FirstOrDefault(x => x.Product.ProductId == request.ProductId);

      // if not exists then it should be a new item
      if (item == null)
      {
        isNewItem = true;
        item = new CartItem()
        {
          Quantity = request.Quantity,
          Product = new Product(request.ProductId)
        };
        cart.CartItems.Add(item);
      }
      else
      {
        // otherwise is updating the current item in the cart
        item.Quantity += request.Quantity;
      }

      cart = _priceCalculator.Execute(cart);
      var result = await cartRepository.UpdateAsync(cart);

      // Todo: refactor to unit of work later
      if (!isNewItem)
        await cartItemRepository.UpdateAsync(item);
      else
        await cartItemRepository.AddAsync(item);

      await UnitOfWork.SaveChangesAsync(cancellationToken);

      return new UpdateItemInCartResponse {Result = cart.ToDto()};
    }
  }
}
