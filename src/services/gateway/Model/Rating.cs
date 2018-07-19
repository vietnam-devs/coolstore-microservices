using System;

namespace VND.CoolStore.Services.ApiGateway.Model
{
		public class Rating
		{
				public Guid Id { get; set; }
				public double Rate { get; set; }
				public int Count { get; set; }
		}
}
