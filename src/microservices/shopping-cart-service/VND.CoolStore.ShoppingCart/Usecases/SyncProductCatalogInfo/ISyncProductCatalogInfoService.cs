using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.ProductCatalog.DataContracts.Event.V1;

namespace VND.CoolStore.ShoppingCart.Usecases.SyncProductCatalogInfo
{
    public interface ISyncProductCatalogInfoService
    {
        Task SyncData(ProductUpdated @event, CancellationToken cancellationToken);
    }
}
