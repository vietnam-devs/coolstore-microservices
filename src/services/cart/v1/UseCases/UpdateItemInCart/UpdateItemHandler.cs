using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.UseCases.v1.Services;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Repository;
using VND.FW.Infrastructure.EfCore.Service;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  public class UpdateItemHandler : CartServiceBase,
    IRequestHandler<UpdateItemInCartRequest, UpdateItemInCartResponse>,
    ICommandService, IQueryService
  {
    private readonly NoTaxCaculator _priceCalculator;

    public UpdateItemHandler(
      ICatalogGateway catalogGateway,
      IUnitOfWorkAsync uow,
      IQueryRepositoryFactory queryRepositoryFactory,
      NoTaxCaculator priceCalculator)
      : base(catalogGateway)
    {
      UnitOfWork = uow;
      QueryRepositoryFactory = queryRepositoryFactory;
      _priceCalculator = priceCalculator;
    }

    public IQueryRepositoryFactory QueryRepositoryFactory { get; }

    public IUnitOfWorkAsync UnitOfWork { get; }

    public override IEfQueryRepository<Domain.Cart> GetQueryRepository()
    {
      return QueryRepositoryFactory.QueryRepository<Domain.Cart>() as IEfQueryRepository<Domain.Cart>;
    }

    public async Task<UpdateItemInCartResponse> Handle(UpdateItemInCartRequest request, CancellationToken cancellationToken)
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

      return new UpdateItemInCartResponse
      {
        Result = GetCartByIdResponse(cart)
      };
    }
  }
}
