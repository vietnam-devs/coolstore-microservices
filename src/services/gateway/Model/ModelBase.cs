using System;
using System.ComponentModel.DataAnnotations;

namespace VND.CoolStore.Services.ApiGateway.Model
{
		public abstract class ModelBase
		{
				[Required]
				public Guid Id { get; set; }
		}
}
