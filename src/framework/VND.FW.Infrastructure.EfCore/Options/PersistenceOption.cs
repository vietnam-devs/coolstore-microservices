using System;
using System.Collections.Generic;
using System.Text;

namespace VND.FW.Infrastructure.EfCore.Options
{
		public class EfCoreOption
		{
				public string FullyQualifiedPrefix { get; set; } = "VND.CoolStore.Services.*";
				public string ShortyQualifiedPrefix { get; set; } = "VND.CoolStore.Services";
		}
}
