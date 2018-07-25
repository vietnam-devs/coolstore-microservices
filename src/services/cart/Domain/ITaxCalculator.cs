namespace VND.CoolStore.Services.Cart.Domain
{
  public interface IPriceCalculator
  {
    Cart Execute(Cart cart);
  }
}
