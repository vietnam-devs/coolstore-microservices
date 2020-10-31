using InventoryService.Domain.Model;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.EfCore;

namespace InventoryService.Infrastructure.Data
{
    public class MainDbContextDesignFactory : DbContextDesignFactoryBase<MainDbContext>
    {
    }

    public class MainDbContext : AppDbContextBase
    {
        private const string Schema = "inventory";

        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension(Consts.UuidGenerator);

            // product
            modelBuilder.Entity<Inventory>().ToTable("inventories", Schema);
            modelBuilder.Entity<Inventory>().HasKey(x => x.Id);
            modelBuilder.Entity<Inventory>().Property(x => x.Id).HasColumnType("uuid")
                .HasDefaultValueSql(Consts.UuidAlgorithm);

            modelBuilder.Entity<Inventory>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

            modelBuilder.Entity<Inventory>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Inventory>().Ignore(x => x.DomainEvents);
        }
    }
}
