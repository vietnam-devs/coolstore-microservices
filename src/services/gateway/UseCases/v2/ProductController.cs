using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VND.CoolStore.Services.ApiGateway.Extensions;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
		/// <summary>
		/// Reference at https://github.com/FabianGosebrink/ASPNETCore-WebAPI-Sample/blob/master/SampleWebApiAspNetCore/Controllers/v1/FoodsController.cs
		/// </summary>
		[ApiVersion("2.0")]
		[Route("api/v{api-version:apiVersion}/products")]
		public class ProductController : Controller
		{
				private readonly IUrlHelper _urlHelper;

				public ProductController(IUrlHelper urlHelper)
				{
						_urlHelper = urlHelper;
				}

				[Auth(Policy = "access_inventory_api")]
				[HttpGet(Name = nameof(GetAllProducts))]
				public ActionResult<IEnumerable<Product>> GetAllProducts([FromQuery] Criterion criterion)
				{
						var products = ProductStub();
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
				public ActionResult<Product> GetProduct(Guid id)
				{
						return ProductStub().FirstOrDefault(x => x.Id == id);
				}

				private IEnumerable<Product> ProductStub()
				{
						return new List<Product>
						{
								new Product
								{
										Id = Guid.NewGuid(),
										Name = "Product 1",
										Desc = "This is a product 1",
										Price = 100.54,
										Rating = new Rating
										{
												Id = Guid.NewGuid(),
												Rate = 4.5,
												Count = 100
										}
								},
								new Product
								{
										Id = Guid.NewGuid(),
										Name = "Product 2",
										Desc = "This is a product 2",
										Price = 120.30,
										Rating = new Rating
										{
												Id = Guid.NewGuid(),
												Rate = 3,
												Count = 20
										}
								},
								new Product
								{
										Id = Guid.NewGuid(),
										Name = "Product 3",
										Desc = "This is a product 3",
										Price = 2.30,
										Rating = new Rating
										{
												Id = Guid.NewGuid(),
												Rate = 3,
												Count = 20
										}
								}
						};
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

				private dynamic ExpandSingleFoodItem(Product item)
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
