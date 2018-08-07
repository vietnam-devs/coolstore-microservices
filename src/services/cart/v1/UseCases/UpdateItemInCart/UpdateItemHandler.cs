using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore.CleanArch;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  public class UpdateItemHandler : EventHandlerBase<UpdateItemInCartRequest, UpdateItemInCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly NoTaxCaculator _priceCalculator;

    public UpdateItemHandler(
      IUnitOfWorkAsync uow,
      IQueryRepositoryFactory qrf,
      ICatalogGateway catalogGateway,
      NoTaxCaculator priceCalculator) : base(uow, qrf)
    {
      _catalogGateway = catalogGateway;
      _priceCalculator = priceCalculator;
    }

    public override async Task<UpdateItemInCartResponse> Handle(UpdateItemInCartRequest request, CancellationToken cancellationToken)
    {
      var cartQueryRepository = QueryRepositoryFactory.QueryEfRepository<Domain.Cart>();
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cartItemRepository = UnitOfWork.Repository<CartItem>();

      var isNewItem = false;
      var cart = await cartQueryRepository.GetFullCart(request.CartId);
      cart = await cart.InitCart(_catalogGateway);

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
        item.Quantity = request.Quantity;
      }

      cart = _priceCalculator.Execute(cart);
      var result = await cartRepository.UpdateAsync(cart);

      // Todo: refactor to unit of work later
      if (!isNewItem)
      {
        await cartItemRepository.UpdateAsync(item);
      }
      else
      {
        await cartItemRepository.AddAsync(item);
      }

      return new UpdateItemInCartResponse { Result = cart.ToCartDto() };
    }
  }
}
