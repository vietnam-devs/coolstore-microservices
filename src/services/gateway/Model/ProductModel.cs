namespace VND.CoolStore.Services.ApiGateway.Model
{
	public class ProductModel : ModelBase
	{
		public string Name { get; set; }
		public string Desc { get; set; }
		public double Price { get; set; }
		public Inventory Availability { get; set; }
		public Rating Rating { get; set; }
	}
}
