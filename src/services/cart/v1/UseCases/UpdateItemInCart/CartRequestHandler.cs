using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Extensions;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore.CleanArch;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  public class CartRequestHandler : TxRequestHandlerBase<UpdateItemInCartRequest, UpdateItemInCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly NoTaxCaculator _priceCalculator;

    public CartRequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf,
      ICatalogGateway catalogGateway, NoTaxCaculator priceCalculator) : base(uow, qrf)
    {
      _catalogGateway = catalogGateway;
      _priceCalculator = priceCalculator;
    }

    public override async Task<UpdateItemInCartResponse> TxHandle(UpdateItemInCartRequest request,
      CancellationToken cancellationToken)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cartItemRepository = UnitOfWork.Repository<CartItem>();

      var isNewItem = false;
      var cart = await QueryRepositoryFactory
        ?.QueryEfRepository<Domain.Cart>()
        ?.GetFullCart(request.CartId)
        ?.ToObservable()
        ?.SelectMany(c => c.InitCart(_catalogGateway));

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

      return new UpdateItemInCartResponse {Result = cart.ToCartDto()};
    }
  }
}
