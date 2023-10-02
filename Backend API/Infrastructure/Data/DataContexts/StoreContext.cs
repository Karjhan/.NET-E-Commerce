using System.Reflection;
using Backend_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.DataContexts;

public class StoreContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    public DbSet<ProductType> ProductTypes { get; set; }
    
    public DbSet<ProductBrand> ProductBrands { get; set; }
    
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(prop => prop.PropertyType == typeof(decimal));
                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                }
            }
        }
    }
}