using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.Authz;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Inventory.v1.Dtos;

namespace VND.CoolStore.Services.Inventory.v1
{
  [ApiVersion("1.0")]
  [Route("api/availability")]
  public class AvailabilityController : Controller
  {
    private readonly IQueryRepositoryFactory _queryRepositoryFactory;

    public AvailabilityController(IQueryRepositoryFactory queryRepositoryFactory)
    {
      _queryRepositoryFactory = queryRepositoryFactory;
    }

    [HttpGet("{id}")]
    [Auth(Policy = "access_inventory_api")]
    public async Task<IActionResult> Get(Guid id)
    {
      var repo = _queryRepositoryFactory.QueryEfRepository<Domain.Inventory>();
      var inv = await repo.FindOneAsync(x => x.Id == id);
      return Ok(inv.ToDto());
    }

    [HttpGet]
    public ActionResult<string> Sample(CancellationToken cancellationToken)
    {
      return "1.0";
    }
  }
}
