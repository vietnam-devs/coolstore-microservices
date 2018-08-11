using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Inventory.Extensions;

namespace VND.CoolStore.Services.Inventory.v1.UseCases.GetInventory
{
  public class RequestHandler : RequestHandlerBase<GetInventoryRequest, GetInventoryResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory queryRepositoryFactory)
      : base(uow, queryRepositoryFactory)
    {
    }

    public override async Task<GetInventoryResponse> Handle(GetInventoryRequest request, CancellationToken cancellationToken)
    {
      var repo = QueryRepositoryFactory?.QueryEfRepository<Domain.Inventory>();
      var inv = await repo.FindOneAsync(x => x.Id == request.InventoryId);
      return new GetInventoryResponse { Result = inv.ToDto() };
    }
  }
}
