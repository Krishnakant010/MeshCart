using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OutboxMessage>(msg =>
        {
            msg.HasKey(x => x.Id);
            msg.HasIndex(x => x.CorrelationId);
            msg.Property(x => x.Type).IsRequired();
            msg.Property(x => x.Content).IsRequired();
            msg.Property(x => x.OccurredOn).IsRequired();
            msg.Property(x => x.ProcessedOn).IsRequired(false);
        });
        modelBuilder.Entity<Order>().Property(o => o.Status).HasConversion<string>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "System";
                    break;
                case EntityState.Modified:
                    entry.Entity.EditedOn = DateTime.UtcNow;
                    entry.Entity.LastEditedBy = "Editorlol";
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}