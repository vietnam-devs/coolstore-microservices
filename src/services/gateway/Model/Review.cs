using System;

namespace VND.CoolStore.Services.ApiGateway.Model
{
	public class Review
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public int Rate { get; set; }
		public string Content { get; set; }
		public string UserName { get; set; }
		public string Title { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
