using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using CloudNativeKit.Utils.Extensions;
using VND.CoolStore.ProductCatalog.DataContracts.Event.V1;
using VND.CoolStore.ShoppingCart.Data;

namespace VND.CoolStore.ShoppingCart.Usecases.SyncProductCatalogInfo
{
    public class SyncProductCatalogInfoService : ISyncProductCatalogInfoService
    {
        private readonly IEfUnitOfWork<ShoppingCartDataContext> _unitOfWork;

        public SyncProductCatalogInfoService(IEfUnitOfWork<ShoppingCartDataContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SyncData(ProductUpdated @event, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.ProductCatalog.ProductCatalog, Guid>();
            var product = await queryRepository.FindOneAsync<ShoppingCartDataContext, Domain.ProductCatalog.ProductCatalog, Guid>(
                    x => x.ProductId == @event.Id.ConvertTo<Guid>());
            if (product != null)
            {
                var repository = _unitOfWork.RepositoryAsync<Domain.ProductCatalog.ProductCatalog, Guid>();
                var updated = product.SyncData(@event);
                await repository.UpdateAsync(updated);
                var rowCount = await _unitOfWork.SaveChangesAsync(cancellationToken);
                if (rowCount <= 0)
                {
                    throw new Exception("Could not sync data.");
                }
            }
        }
    }
}
