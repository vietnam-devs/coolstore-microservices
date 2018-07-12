using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Extensions;
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
		public class ProductController : FW.Infrastructure.AspNetCore.ControllerBase
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

				[Auth(Policy = "access_inventory_api")]
				[HttpGet(Name = nameof(GetAllProducts))]
				public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProducts([FromQuery] Criterion criterion)
				{
						InitRestClientWithOpenTracing();

						var getProductsEndPoint = $"{_catalogServiceUri}/api/v1/products";
						var products = await RestClient.GetAsync<List<ProductModel>>(getProductsEndPoint);
						var numberOfProducts = products.Count();

						var paginationMetadata = new
						{
								totalCount = numberOfProducts,
								pageSize = criterion.PageSize,
								currentPage = criterion.CurrentPage,
								totalPages = criterion.GetTotalPages(numberOfProducts)
						};

						Response.Headers.Add("X-Pagination",
							Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

						var links = CreateLinksForCollection(criterion, numberOfProducts);

						var toReturn = products.Select(x => ExpandSingleFoodItem(x));

						return Ok(new
						{
								value = toReturn,
								links
						});
				}

				[HttpGet]
				[Route("{id:guid}", Name = nameof(GetProduct))]
				public async Task<ActionResult<ProductModel>> GetProduct(Guid id)
				{
						InitRestClientWithOpenTracing();

						var getProductEndPoint = $"{_catalogServiceUri}/api/v1/products/{id}";
						var product = await RestClient.GetAsync<List<ProductModel>>(getProductEndPoint);

						return Ok(product);
				}

				private List<LinkItem> CreateLinksForCollection(Criterion criterion, int totalCount)
				{
						var links = new List<LinkItem>();

						// self 
						links.Add(
							new LinkItem(_urlHelper.Link(nameof(GetAllProducts), new
							{
									pagecount = criterion.PageSize,
									page = criterion.CurrentPage,
									orderby = criterion.SortBy
							}), "self", "GET"));

						links.Add(new LinkItem(_urlHelper.Link(nameof(GetAllProducts), new
						{
								pagecount = criterion.PageSize,
								page = 1,
								orderby = criterion.SortBy
						}), "first", "GET"));

						links.Add(new LinkItem(_urlHelper.Link(nameof(GetAllProducts), new
						{
								pagecount = criterion.PageSize,
								page = criterion.GetTotalPages(totalCount),
								orderby = criterion.SortBy
						}), "last", "GET"));

						if (criterion.HasNext(totalCount))
						{
								links.Add(new LinkItem(_urlHelper.Link(nameof(GetAllProducts), new
								{
										pagecount = criterion.PageSize,
										page = criterion.CurrentPage + 1,
										orderby = criterion.SortBy
								}), "next", "GET"));
						}

						if (criterion.HasPrevious())
						{
								links.Add(new LinkItem(_urlHelper.Link(nameof(GetAllProducts), new
								{
										pagecount = criterion.PageSize,
										page = criterion.CurrentPage - 1,
										orderby = criterion.SortBy
								}), "previous", "GET"));
						}

						return links;
				}

				private dynamic ExpandSingleFoodItem(ProductModel item)
				{
						var links = GetLinks(item.Id);
						var resourceToReturn = item.ToDynamic() as IDictionary<string, object>;
						resourceToReturn.Add("links", links);

						return resourceToReturn;
				}

				private IEnumerable<LinkItem> GetLinks(Guid id)
				{
						var links = new List<LinkItem>();

						links.Add(
							new LinkItem(_urlHelper.Link(nameof(GetProduct), new { id }),
								"self",
								"GET"));

						return links;
				}
		}
}
