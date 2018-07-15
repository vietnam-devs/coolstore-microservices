using Microsoft.AspNetCore.Mvc;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.FW.Infrastructure.AspNetCore
{
		public abstract class ControllerBase : Controller
		{
				protected static RestClient RestClient;
				protected ControllerBase(RestClient rest)
				{
						RestClient = rest;
				}

				protected void InitRestClientWithOpenTracing()
				{
						RestClient.SetOpenTracingInfo(HttpContext.Request.GetOpenTracingInfo());
				}
		}
}
