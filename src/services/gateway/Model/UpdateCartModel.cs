using System.ComponentModel.DataAnnotations;

namespace VND.CoolStore.Services.ApiGateway.Model
{
		public class UpdateCartModel
		{
				[Required]
				public int Quantity { get; set; }
		}
}