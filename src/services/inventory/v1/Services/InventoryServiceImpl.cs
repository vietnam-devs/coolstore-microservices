using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using NetCoreKit.Utils.Extensions;
using VND.CoolStore.Services.Inventory.v1.Extensions;
using VND.CoolStore.Services.Inventory.v1.Grpc;

namespace VND.CoolStore.Services.Inventory.v1.Services
{
    public class InventoryServiceImpl : InventoryService.InventoryServiceBase
    {
        private readonly IQueryRepositoryFactory _queryRepositoryFactory;
        private readonly IServiceProvider _resolver;

        public InventoryServiceImpl(IServiceProvider resolver)
        {
            _resolver = resolver;
            _queryRepositoryFactory = resolver.GetService<IQueryRepositoryFactory>();
        }

        public override async Task<GetInventoriesResponse> GetInventories(Empty request, ServerCallContext context)
        {
            var repo = _queryRepositoryFactory.QueryEfRepository<Domain.Inventory>();

            var inventories = await repo.ListAsync();
            var dtos = inventories.Select(i => i.ToDto());
            var response = new GetInventoriesResponse();
            response.Inventories.AddRange(dtos);

            return response;
        }

        public override async Task<GetInventoryResponse> GetInventory(GetInventoryRequest request,
            ServerCallContext context)
        {
            var repo = _queryRepositoryFactory.QueryEfRepository<Domain.Inventory>();

            var inv = await repo.FindOneAsync(x => x.Id == request.Id.ConvertTo<Guid>());
            var response = new GetInventoryResponse
            {
                Result = inv.ToDto()
            };

            return response;
        }

        public override async Task<DbMigrationResponse> DbMigration(Empty request, ServerCallContext context)
        {
            var ok = await Task.Run(() => _resolver.MigrateDbContext() != null);
            return await Task.FromResult(new DbMigrationResponse {Ok = ok});
        }
    }
}
