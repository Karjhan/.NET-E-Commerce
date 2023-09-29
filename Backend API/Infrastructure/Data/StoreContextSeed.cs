using System.Reflection;
using System.Text.Json;
using Backend_API.DataContexts;
using Backend_API.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        // var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!context.ProductBrands.Any())
        {
            var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            await context.ProductBrands.AddRangeAsync(brands);
        }
        if (!context.ProductTypes.Any())
        {
            var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
            var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            await context.ProductTypes.AddRangeAsync(types);
        }
        if (!context.Products.Any())
        {
            var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            await context.Products.AddRangeAsync(products);
        }

        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync();
        }
    }
}