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
    protected readonly IEfQueryRepository<TEntity> QueryRepository = null;
    protected readonly IEfRepositoryAsync<TEntity> MutateRepository = null;

    public CrudControllerBase(
        IEfQueryRepository<TEntity> queryRepository,
        IEfRepositoryAsync<TEntity> mutateRepository)
    {
      QueryRepository = queryRepository;
      MutateRepository = mutateRepository;
    }

    [HttpGet(Name = nameof(GetAllItems))]
    public async Task<ActionResult<PaginatedItem<TEntity>>> GetAllItems([FromQuery] Criterion criterion)
    {
      criterion = criterion ?? new Criterion();
      return await QueryRepository.QueryAsync(criterion, entity => entity);
    }

    [HttpGet("{id}", Name = nameof(GetItem))]
    public async Task<ActionResult<TEntity>> GetItem(Guid id)
    {
      return await QueryRepository.GetByIdAsync(id);
    }

    [HttpPost(Name = nameof(PostItem))]
    public async Task<TEntity> PostItem([FromBody] TEntity entity)
    {
      return await MutateRepository.AddAsync(entity);
    }

    [HttpPut("{id}", Name = nameof(PutItem))]
    public async Task<TEntity> PutItem(int id, [FromBody] TEntity entity)
    {
      return await MutateRepository.UpdateAsync(entity);
    }

    [HttpDelete("{id}", Name = nameof(DeleteItem))]
    public async Task<TEntity> DeleteItem(Guid id)
    {
      return await MutateRepository.DeleteAsync(await QueryRepository.GetByIdAsync(id));
    }
  }
}
