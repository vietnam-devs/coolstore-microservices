using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Service;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Catalog.GetProductById;
using VND.CoolStore.Shared.Catalog.GetProducts;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;

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
    private readonly ICatalogService _catalogService;

    public ProductController(
      ICatalogService catalogService,
      IUrlHelper urlHelper)
    {
      _urlHelper = urlHelper;
      _catalogService = catalogService;
    }

    [HttpGet(Name = nameof(GetAllProducts))]
    [Auth(Policy = "access_catalog_api")]
    [SwaggerOperation(Tags = new[] { "catalog-service" })]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProducts([FromQuery] Criterion criterion)
    {
      IEnumerable<GetProductsResponse> products = await _catalogService.GetProductsAsync(new GetProductsRequest());
      int numberOfProducts = products.Count();

      Response.AddPaginateInfo(criterion, numberOfProducts);

      List<LinkItem> links = _urlHelper.CreateLinksForCollection(nameof(GetAllProducts), criterion, numberOfProducts);

      IEnumerable<dynamic> toReturn = products.Select(x => _urlHelper.ExpandSingleItem(nameof(GetProduct), x));

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
    public async Task<ActionResult<GetProductByIdResponse>> GetProduct(Guid id)
    {
      GetProductByIdResponse product = await _catalogService.GetProductByIdAsync(new GetProductByIdRequest { Id = id });
      return Ok(product);
    }
  }
}
