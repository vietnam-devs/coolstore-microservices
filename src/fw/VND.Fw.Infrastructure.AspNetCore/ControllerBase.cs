using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VND.Fw.Domain;
using VND.Fw.Infrastructure.EfCore.Extensions;
using VND.Fw.Infrastructure.EfCore.Repository;

namespace VND.Fw.Infrastructure.AspNetCore
{
  [ApiController]
  public abstract class EvtControllerBase : Controller
  {
    protected IMediator Eventor;

    protected EvtControllerBase(IMediator eventor)
    {
      Eventor = eventor;
    }
  }

  [ApiController]
  public abstract class EfCoreControllerBase<TEntity> : Controller
    where TEntity : EntityBase
  {
    protected readonly IEfQueryRepository<TEntity> QueryRepository;
    protected readonly IEfRepositoryAsync<TEntity> MutateRepository;

    protected EfCoreControllerBase(
        IEfQueryRepository<TEntity> queryRepository,
        IEfRepositoryAsync<TEntity> mutateRepository)
    {
      QueryRepository = queryRepository;
      MutateRepository = mutateRepository;
    }
  }

  /// <summary>
  /// https://github.com/FabianGosebrink/ASPNETCore-WebAPI-Sample/blob/master/SampleWebApiAspNetCore/Controllers/v1/FoodsController.cs
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  [ApiController]
  public abstract class CrudControllerBase<TEntity> : EfCoreControllerBase<TEntity>
      where TEntity : EntityBase
  {
    protected CrudControllerBase(
        IEfQueryRepository<TEntity> queryRepository,
        IEfRepositoryAsync<TEntity> mutateRepository)
      : base(queryRepository, mutateRepository)
    {
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
    public async Task<TEntity> PostItem(TEntity entity)
    {
      return await MutateRepository.AddAsync(entity);
    }

    [HttpPut("{id}", Name = nameof(PutItem))]
    public async Task<TEntity> PutItem(int id, TEntity entity)
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
