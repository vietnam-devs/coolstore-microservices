using System;

namespace VND.CoolStore.ShoppingCart.DataContracts.V1
{
    /// <summary>
    /// Ref https://stackoverflow.com/questions/5898988/map-string-to-guid-with-dapper
    /// </summary>
    public partial class CartWithProductsRow
    {
        public Guid CartIdGuid { get; set; }
        public Guid ProductIdGuid { get; set; }
    }
}
