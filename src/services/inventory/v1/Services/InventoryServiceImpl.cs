using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreKit.Utils.Extensions;
using VND.CoolStore.Services.Inventory.v1.Extensions;
using VND.CoolStore.Services.Inventory.v1.Grpc;

namespace VND.CoolStore.Services.Inventory.v1.Services
{
    public class InventoryServiceImpl : InventoryService.InventoryServiceBase
    {
        private readonly ILogger<InventoryServiceImpl> _logger;
        //private readonly IQueryRepositoryFactory _queryRepositoryFactory;
        private readonly IServiceProvider _resolver;
        private readonly List<Domain.Inventory> _inMemoryDb = new List<Domain.Inventory>();

        public InventoryServiceImpl(IServiceProvider resolver)
        {
            _resolver = resolver;
            _logger = resolver.GetService<ILoggerFactory>()?.CreateLogger<InventoryServiceImpl>();
            //_queryRepositoryFactory = resolver.GetService<IQueryRepositoryFactory>();
            SeedData(_inMemoryDb);
        }

        private void SeedData(List<Domain.Inventory> inMemoryDb)
        {
            inMemoryDb.Add(
                new Domain.Inventory(new Guid("25e6ba6e-fddb-401d-99b2-33ddc9f29322"))
                {
                    Link = "https://www.nashtechglobal.com/visit-vietnam",
                    Location = "Ho Chi Minh City, Vietnam",
                    Quantity = 1000
                });

            inMemoryDb.Add(
                new Domain.Inventory(new Guid("cab3818f-e459-412f-972f-d4b2d36aa735"))
                {
                    Link = "https://www.microsoft.com/en-us/build",
                    Location = "Washington State Convention Center, Seattle, WA",
                    Quantity = 10000
                });
        }

        public override async Task<GetInventoriesResponse> GetInventories(Empty request, ServerCallContext context)
        {
            try
            {
                /*var repo = _queryRepositoryFactory.QueryEfRepository<Domain.Inventory>();
                var inventories = await repo.ListAsync();
                var dtos = inventories.Select(i => i.ToDto());
                var response = new GetInventoriesResponse();
                response.Inventories.AddRange(dtos);
                return response;*/

                var dtos = _inMemoryDb.Select(i => i.ToDto());
                var response = new GetInventoriesResponse();
                response.Inventories.AddRange(dtos);
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<GetInventoryResponse> GetInventory(GetInventoryRequest request,
            ServerCallContext context)
        {
            try
            {
                /*var repo = _queryRepositoryFactory.QueryEfRepository<Domain.Inventory>();
                var inv = await repo.FindOneAsync(x => x.Id == request.Id.ConvertTo<Guid>());
                var response = new GetInventoryResponse
                {
                    Result = inv.ToDto()
                };
                return response;*/

                var inv = _inMemoryDb.FirstOrDefault(i => i.Id == request.Id.ConvertTo<Guid>());
                var response = new GetInventoryResponse
                {
                    Result = inv.ToDto()
                };
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
    }
}
