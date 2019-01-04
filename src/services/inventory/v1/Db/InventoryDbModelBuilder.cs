using System;
using Microsoft.EntityFrameworkCore;
using NetCoreKit.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Inventory.v1.Db
{
    public class InventoryDbModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Inventory>().HasData(
                new Domain.Inventory(
                    new Guid("25E6BA6E-FDDB-401D-99B2-33DDC9F29322"))
                {
                    Link = "http://nashtechglobal.com",
                    Location = "London, UK",
                    Quantity = 100
                },
                new Domain.Inventory(
                    new Guid("CAB3818F-E459-412F-972F-D4B2D36AA735"))
                {
                    Link = "http://nashtechvietnam.com",
                    Location = "Ho Chi Minh City, Vietnam",
                    Quantity = 1000
                }
            );
        }
    }
}
