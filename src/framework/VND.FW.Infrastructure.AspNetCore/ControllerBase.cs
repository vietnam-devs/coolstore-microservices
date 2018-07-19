using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.EfCore.Extensions;
using VND.FW.Infrastructure.EfCore.Repository;

namespace VND.FW.Infrastructure.AspNetCore
{
  public abstract class ControllerBase : Controller
  {

  }

  public abstract class ProxyControllerBase : ControllerBase
  {
    protected static RestClient RestClient;
    protected ProxyControllerBase(RestClient rest)
    {
      RestClient = rest;
    }

    protected void InitRestClientWithOpenTracing()
    {
      RestClient.SetOpenTracingInfo(HttpContext.Request.GetOpenTracingInfo());
    }
  }

  public abstract class CrudControllerBase<TEntity> : ControllerBase
      where TEntity : EntityBase
  {
    private readonly IEfQueryRepository<TEntity> _queryRepository = null;
    private readonly IEfRepositoryAsync<TEntity> _mutateRepository = null;

    public CrudControllerBase(
        IEfQueryRepository<TEntity> queryRepository,
        IEfRepositoryAsync<TEntity> mutateRepository)
    {
      _queryRepository = queryRepository;
      _mutateRepository = mutateRepository;
    }

    // GET api/values
    [HttpGet(Name = nameof(GetAllItems))]
    public async Task<ActionResult<PaginatedItem<TEntity>>> GetAllItems([FromQuery] Criterion criterion)
    {
      if (criterion == null)
      {
        criterion = new Criterion();
      }

      return await _queryRepository.QueryAsync(criterion, entity => entity);
    }

    // GET api/values/5
    [HttpGet("{id}", Name = nameof(GetItem))]
    public async Task<ActionResult<TEntity>> GetItem(Guid id)
    {
      return await _queryRepository.GetByIdAsync(id);
    }

    // POST api/values
    [HttpPost(Name = nameof(PostItem))]
    public async Task<TEntity> PostItem([FromBody] TEntity entity)
    {
      return await _mutateRepository.AddAsync(entity);
    }

    // PUT api/values/5
    [HttpPut("{id}", Name = nameof(PutItem))]
    public async Task<TEntity> PutItem(int id, [FromBody] TEntity entity)
    {
      return await _mutateRepository.UpdateAsync(entity);
    }

    // DELETE api/values/5
    [HttpDelete("{id}", Name = nameof(DeleteItem))]
    public async Task<TEntity> DeleteItem(Guid id)
    {
      return await _mutateRepository.DeleteAsync(await _queryRepository.GetByIdAsync(id));
    }
  }
}
