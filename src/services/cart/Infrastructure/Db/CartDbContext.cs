using Microsoft.EntityFrameworkCore;
using NetCoreKit.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Cart.Infrastructure.Db
{
    public class CartDbContext : AppDbContext
    {
        public CartDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
