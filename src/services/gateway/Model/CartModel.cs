using System;
using System.ComponentModel.DataAnnotations;
using VND.FW.Infrastructure.AspNetCore;

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
