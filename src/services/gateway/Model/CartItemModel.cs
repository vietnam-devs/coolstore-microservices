namespace VND.CoolStore.Services.ApiGateway.Model
{
  public class CartItemModel
  {
    public double Price { get; set; }
    public int Quantity { get; set; }
    public double PromoSavings { get; set; }
    public ProductModel Product { get; set; }
  }
}
