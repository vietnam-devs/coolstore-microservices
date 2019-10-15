using System;

namespace VND.CoolStore.ShoppingCart.DataContracts.Dto.V1
{
    /// <summary>
    /// Ref https://stackoverflow.com/questions/5898988/map-string-to-guid-with-dapper
    /// Those fields are for dummy mapping from the database, we don't use it in the API output
    /// </summary>
    public partial class CartWithProductsRow
    {
        [NonSerialized]
        public Guid CartIdGuid;

        [NonSerialized]
        public Guid UserIdGuid;

        [NonSerialized]
        public Guid ProductIdGuid;

        [NonSerialized]
        public Guid InventoryIdGuid;
    }
}
