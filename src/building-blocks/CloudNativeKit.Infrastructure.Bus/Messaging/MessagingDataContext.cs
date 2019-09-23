using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudNativeKit.Infrastructure.Bus.Messaging
{
    public class MessagingDataContext : DbContext
    {
        public DbSet<Outbox> OutBoxes { get; set; }

        public MessagingDataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class OutboxTypeConfiguration : IEntityTypeConfiguration<Outbox>
    {
        public void Configure(EntityTypeBuilder<Outbox> builder)
        {
            builder.ToTable("Outboxes", "message");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
        }
    }
}
