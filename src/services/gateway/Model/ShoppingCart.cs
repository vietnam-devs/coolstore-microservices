using System.Collections.Generic;

namespace VND.CoolStore.Services.ApiGateway.Model
{
		public class ShoppingCart
		{
				public double CartItemTotal { get; set; }
				public double CartItemPromoSavings { get; set; }
				public double ShippingTotal { get; set; }
				public double ShippingPromoSavings { get; set; }
				public double CartTotal { get; set; }
				public ICollection<ShoppingCartItem> ShoppingCartItemList { get; set; }
		}
}
