using System;

namespace VND.CoolStore.Services.ApiGateway.Model
{
	public class Inventory
	{
		public Guid Id { get; set; }
		public int Quantity { get; set; }
		public string Location { get; set; }
		public string Link { get; set; }
	}
}
