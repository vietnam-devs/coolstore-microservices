using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.EfCore;
using ProductCatalogService.Domain.Model;

namespace ProductCatalogService.Infrastructure.Persistence
{
    public class MainDbContextDesignFactory : DbContextDesignFactoryBase<MainDbContext>
    {
    }

    public class MainDbContext : AppDbContextBase
    {
        private const string Schema = "catalog";

        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension(Consts.UuidGenerator);

            // product
            modelBuilder.Entity<Product>().ToTable("products", Schema);
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnType("uuid")
                .HasDefaultValueSql(Consts.UuidAlgorithm);

            modelBuilder.Entity<Product>().Property(x => x.InventoryId).HasColumnType("uuid");
            modelBuilder.Entity<Product>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

            modelBuilder.Entity<Product>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Product>().Ignore(x => x.DomainEvents);

            // category
            modelBuilder.Entity<Category>().ToTable("categories", Schema);
            modelBuilder.Entity<Category>().HasKey(x => x.Id);
            modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnType("uuid")
                .HasDefaultValueSql(Consts.UuidAlgorithm);

            modelBuilder.Entity<Category>().Ignore(x => x.DomainEvents);

            // relationship
            modelBuilder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .IsRequired();
        }
    }
}
