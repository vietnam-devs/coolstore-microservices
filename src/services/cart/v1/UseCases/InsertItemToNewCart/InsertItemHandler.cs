using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.UseCases.v1.Services;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Service;

namespace VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart
{
  public class InsertItemHandler : CartServiceBase,
    IRequestHandler<InsertItemToNewCartRequest, InsertItemToNewCartResponse>,
    ICommandService, IQueryService
  {
    private readonly NoTaxCaculator _priceCalculator;

    public InsertItemHandler(
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

    public async Task<InsertItemToNewCartResponse> Handle(InsertItemToNewCartRequest request, CancellationToken cancellationToken)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();

      var cart = new Domain.Cart();
      cart.InsertItemToCart(new CartItem
      {
        Product = new Product(request.ProductId),
        PromoSavings = 0.0D,
        Quantity = request.Quantity
      });

      cart = await InitCart(cart, populatePrice: true);
      cart = _priceCalculator.Execute(cart);

      await cartRepository.AddAsync(cart);

      return new InsertItemToNewCartResponse
      {
        Result = GetCartByIdResponse(cart)
      };
    }
  }
}
