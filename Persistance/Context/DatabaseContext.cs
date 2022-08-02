using Application.Interfaces.IContext;
using Domain.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Context
{
    public class DatabaseContext : DbContext , IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)   
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //builder.Entity<User>().Property<DateTime?>("InsertTime");
            //builder.Entity<User>().Property<DateTime?>("UpdateTime");

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("InsertTime").HasDefaultValue(DateTime.Now);
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                    modelBuilder.Entity(entityType.Name).Property<bool>("IsRemoved").HasDefaultValue(false);
                }
            }

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {


            var modifiedEntries = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified ||
                p.State == EntityState.Added ||
                p.State == EntityState.Deleted
                );
            foreach (var item in modifiedEntries)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
                if (entityType != null)
                {
                    var inserted = entityType.FindProperty("InsertTime");
                    var updated = entityType.FindProperty("UpdateTime");
                    var RemoveTime = entityType.FindProperty("RemoveTime");
                    var IsRemoved = entityType.FindProperty("IsRemoved");
                    if (item.State == EntityState.Added && inserted != null)
                    {
                        item.Property("InsertTime").CurrentValue = DateTime.Now;
                    }
                    if (item.State == EntityState.Modified && updated != null)
                    {
                        item.Property("UpdateTime").CurrentValue = DateTime.Now;
                    }

                    if (item.State == EntityState.Deleted && RemoveTime != null && IsRemoved != null)
                    {
                        item.Property("RemoveTime").CurrentValue = DateTime.Now;
                        item.Property("IsRemoved").CurrentValue = true;
                        item.State = EntityState.Modified;
                    }
                }

            }

            return base.SaveChanges();
        }
    }
}
