using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.Model
{
		public class InventoryModel : ModelBase
		{
				public string Location { get; set; }
				public int Quantity { get; set; }
				public string Link { get; set; }
		}
}
