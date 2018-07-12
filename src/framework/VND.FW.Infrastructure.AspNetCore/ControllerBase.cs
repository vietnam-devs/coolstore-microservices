using Microsoft.AspNetCore.Mvc;

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
