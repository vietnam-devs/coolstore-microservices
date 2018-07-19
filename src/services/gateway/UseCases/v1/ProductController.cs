using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
		/// <summary>
		/// Reference at https://github.com/FabianGosebrink/ASPNETCore-WebAPI-Sample/blob/master/SampleWebApiAspNetCore/Controllers/v1/FoodsController.cs
		/// </summary>
		[ApiVersion("1.0")]
		[Route("api/v{api-version:apiVersion}/products")]
		public class ProductController : ProxyControllerBase
		{
				private readonly IUrlHelper _urlHelper;
				private readonly string _catalogServiceUri;

				public ProductController(RestClient restClient, IUrlHelper urlHelper, IHostingEnvironment env)
						: base(restClient)
				{
						_urlHelper = urlHelper;
						_catalogServiceUri = env.IsDevelopment()
								? "http://localhost:5002"
								: $"http://{Environment.GetEnvironmentVariable("CATALOG_SERVICE_SERVICE_HOST")}:{Environment.GetEnvironmentVariable("IDP_SERVICE_SERVICE_PORT")}";

						
				}

				[HttpGet(Name = nameof(GetAllProducts))]
				[Auth(Policy = "access_catalog_api")]
				[SwaggerOperation(Tags = new[] { "catalog-service" })]
				public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProducts([FromQuery] Criterion criterion)
				{
						InitRestClientWithOpenTracing();

						var getProductsEndPoint = $"{_catalogServiceUri}/api/v1/products";
						var products = await RestClient.GetAsync<List<ProductModel>>(getProductsEndPoint);
						var numberOfProducts = products.Count();

						Response.AddPaginateInfo(criterion, numberOfProducts);

						var links = _urlHelper.CreateLinksForCollection(nameof(GetAllProducts), criterion, numberOfProducts);

						var toReturn = products.Select(x => _urlHelper.ExpandSingleItem(nameof(GetProduct), x));

						return Ok(new
						{
								value = toReturn,
								links
						});
				}

				[HttpGet]
				[Auth(Policy = "access_catalog_api")]
				[SwaggerOperation(Tags = new[] { "catalog-service" })]
				[Route("{id:guid}", Name = nameof(GetProduct))]
				public async Task<ActionResult<ProductModel>> GetProduct(Guid id)
				{
						InitRestClientWithOpenTracing();

						var getProductEndPoint = $"{_catalogServiceUri}/api/v1/products/{id}";
						var product = await RestClient.GetAsync<List<ProductModel>>(getProductEndPoint);

						return Ok(product);
				}
		}
}
