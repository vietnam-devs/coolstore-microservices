using System.ComponentModel;
using CloudNativeKit.Domain;

namespace VND.CoolStore.ProductCatalog.DataContracts.Event.V1
{
    /*[DefaultValue("DefaultReflection")]
    public static partial class CatalogReflection
    {
    }*/

    public partial class ProductUpdated : IntegrationEventBase
    {   
    }

    public partial class ProductDeleted : IntegrationEventBase
    {
    }
}
