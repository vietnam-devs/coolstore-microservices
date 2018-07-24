namespace VND.CoolStore.Services.Cart.Domain
{
  public interface ITaxCalculator
  {
    Cart Execute(Cart cart);
  }
}
