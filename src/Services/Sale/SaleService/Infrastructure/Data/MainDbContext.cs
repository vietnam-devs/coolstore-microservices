using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.EfCore;
using SaleService.Domain.Model;

namespace SaleService.Infrastructure.Data
{
    public class MainDbContextDesignFactory : DbContextDesignFactoryBase<MainDbContext>
    {
    }

    public class MainDbContext : AppDbContextBase
    {
        private const string Schema = "sale";

        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderItem> OrderItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension(Consts.UuidGenerator);

            // order
            modelBuilder.Entity<Order>().ToTable("orders", Schema);
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnType("uuid")
                .HasDefaultValueSql(Consts.UuidAlgorithm);

            modelBuilder.Entity<Order>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

            modelBuilder.Entity<Order>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Order>().Ignore(x => x.DomainEvents);

            // order item
            modelBuilder.Entity<OrderItem>().ToTable("order_items", Schema);
            modelBuilder.Entity<OrderItem>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderItem>().Property(x => x.Id).HasColumnType("uuid")
                .HasDefaultValueSql(Consts.UuidAlgorithm);

            modelBuilder.Entity<OrderItem>().Property(x => x.ProductId).HasColumnType("uuid");
            modelBuilder.Entity<OrderItem>().Property(x => x.InventoryId).HasColumnType("uuid");
            modelBuilder.Entity<OrderItem>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

            modelBuilder.Entity<OrderItem>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<OrderItem>().Ignore(x => x.DomainEvents);

            // relationship
            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .IsRequired();
        }
    }
}
