using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart
{
  public class InsertItemHandler : EventHandlerBase<InsertItemToNewCartRequest, InsertItemToNewCartResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    private readonly NoTaxCaculator _priceCalculator;

    public InsertItemHandler(
      IUnitOfWorkAsync uow,
      IQueryRepositoryFactory qrf,
      ICatalogGateway catalogGateway,
      NoTaxCaculator priceCalculator) : base(uow, qrf)
    {
      _catalogGateway = catalogGateway;
      _priceCalculator = priceCalculator;
    }

    public override async Task<InsertItemToNewCartResponse> Handle(InsertItemToNewCartRequest request, CancellationToken cancellationToken)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();

      var cart = new Domain.Cart().InsertItemToCart(new CartItem
      {
        Product = new Product(request.ProductId),
        PromoSavings = 0.0D,
        Quantity = request.Quantity
      });

      cart = await cart.InitCart(_catalogGateway, isPopulatePrice: true);
      cart = _priceCalculator.Execute(cart);

      await cartRepository.AddAsync(cart);

      return new InsertItemToNewCartResponse { Result = cart.ToCartDto() };
    }
  }
}
