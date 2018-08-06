using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Dtos;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.Infrastructure.Services;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Repository;
using VND.FW.Infrastructure.EfCore.Service;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart.Impl
{
  public class UpdateItemService : CartServiceBase, IUpdateItemService, ICommandService, IQueryService
  {
    public UpdateItemService(
      ICatalogGateway catalogGateway,
      IUnitOfWorkAsync uow,
      IQueryRepositoryFactory queryRepositoryFactory)
      : base(catalogGateway)
    {
      UnitOfWork = uow;
      QueryRepositoryFactory = queryRepositoryFactory;
    }

    public IUnitOfWorkAsync UnitOfWork { get; }

    public IQueryRepositoryFactory QueryRepositoryFactory { get; }

    public PriceCalculatorContext PriceCalculatorContext { get; set; }

    public override IEfQueryRepository<Domain.Cart> GetQueryRepository()
    {
      return QueryRepositoryFactory.QueryRepository<Domain.Cart>() as IEfQueryRepository<Domain.Cart>;
    }

    public async Task<CartDto> Execute(UpdateItemInCartRequest request)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cartItemRepository = UnitOfWork.Repository<CartItem>();

      var isNewItem = false;
      var cart = await GetCart(request.CartId);
      cart = await InitCart(cart);

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

      cart = PriceCalculatorContext.Execute(cart);
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

      return GetCartByIdResponse(cart);
    }
  }
}
