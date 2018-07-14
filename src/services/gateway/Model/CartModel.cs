using System;
using System.ComponentModel.DataAnnotations;

namespace VND.CoolStore.Services.ApiGateway.Model
{
		public class CartModel : ModelBase
		{
				[Required]
				public Guid ItemId { get; set; }

				[Required]
				public int Quantity { get; set; }
		}
}
