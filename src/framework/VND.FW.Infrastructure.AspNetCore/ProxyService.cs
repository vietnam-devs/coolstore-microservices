using Microsoft.AspNetCore.Http;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.FW.Infrastructure.AspNetCore
{
		public abstract class ProxyServiceBase
		{
				protected readonly RestClient RestClient;
				private readonly HttpContext _context;
				protected ProxyServiceBase(
						RestClient rest,
			      IHttpContextAccessor httpContextAccessor)
				{
						_context = httpContextAccessor.HttpContext;
						RestClient = rest;
						RestClient.SetOpenTracingInfo(_context?.Request.GetOpenTracingInfo());
				}
		}
}
