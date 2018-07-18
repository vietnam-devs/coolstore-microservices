using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using VND.FW.Infrastructure.EfCore.Db;
using VND.FW.Infrastructure.EfCore.Migration;

namespace VND.CoolStore.Services.Inventory.SeedData
{
		public class InventorySeeder : SeedDataBase<ApplicationDbContext>
		{
				public InventorySeeder(IConfiguration configuration) : base(configuration)
				{
				}

				public override async Task SeedAsync(ApplicationDbContext context)
				{
						var inventorySet = context.Set<Domain.Inventory>();

						if (inventorySet.Count() > 0) return;

						var inventory1 = new Domain.Inventory(
								new System.Guid("25E6BA6E-FDDB-401D-99B2-33DDC9F29322"))
						{
								Link = "http://nashtechglobal.com",
								Location = "London, UK",
								Quantity = 100,
						};

						var inventory2 = new Domain.Inventory(
								new System.Guid("CAB3818F-E459-412F-972F-D4B2D36AA735"))
						{
								Link = "http://nashtechvietnam.com",
								Location = "Ho Chi Minh City, Vietnam",
								Quantity = 1000,
						};

						if (!inventorySet.Any(x => x.Id == inventory1.Id)) {
								await inventorySet.AddAsync(inventory1);
						}

						if (!inventorySet.Any(x => x.Id == inventory2.Id))
						{
								await inventorySet.AddAsync(inventory2);
						}

						await context.SaveChangesAsync();
				}
		}
}
