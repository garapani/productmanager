using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ProductManager.Data
{
    public class ProductDBContext : DbContext
    {
        public ProductDBContext(DbContextOptions<ProductDBContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }

        public DbSet<ProductAnalytics> ProductAnalytics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasQueryFilter(f => f.IsDeleted == false);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.UpdateSoftDeleteProperty();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.UpdateSoftDeleteProperty();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            this.UpdateSoftDeleteProperty();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateSoftDeleteProperty();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateSoftDeleteProperty()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity.GetType() == typeof(Product)))
            {
                switch (entry.State)
                {
                    
                    case EntityState.Added:                        
                        entry.CurrentValues["CreatedAt"] = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues["UpdatedAt"] = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.CurrentValues["UpdatedAt"] = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
