using Backend_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.DataContexts;

public class StoreContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    public StoreContext(DbContextOptions options) : base(options)
    {
        
    }
}