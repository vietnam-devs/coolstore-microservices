using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Extensions;
using VND.CoolStore.Services.ApiGateway.Model;
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
  public class ProductController : ProxyControllerBase
  {
    private readonly IUrlHelper _urlHelper;
    private readonly string _catalogServiceUri;

    public ProductController(RestClient restClient, IUrlHelper urlHelper, IConfiguration config, IHostingEnvironment env)
        : base(restClient)
    {
      _urlHelper = urlHelper;
      _catalogServiceUri = config.GetHostUri(env, "Catalog");
    }

    [HttpGet(Name = nameof(GetAllProducts))]
    [Auth(Policy = "access_catalog_api")]
    [SwaggerOperation(Tags = new[] { "catalog-service" })]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProducts([FromQuery] Criterion criterion)
    {
      InitRestClientWithOpenTracing();

      string getProductsEndPoint = $"{_catalogServiceUri}/api/v1/products";
      List<ProductModel> products = await RestClient.GetAsync<List<ProductModel>>(getProductsEndPoint);
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
    public async Task<ActionResult<ProductModel>> GetProduct(Guid id)
    {
      InitRestClientWithOpenTracing();

      string getProductEndPoint = $"{_catalogServiceUri}/api/v1/products/{id}";
      List<ProductModel> product = await RestClient.GetAsync<List<ProductModel>>(getProductEndPoint);

      return Ok(product);
    }
  }
}
