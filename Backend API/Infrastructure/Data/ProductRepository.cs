using Backend_API.DataContexts;
using Backend_API.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private StoreContext _context { get; set; }

    public ProductRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.Include(product => product.ProductBrand).Include(product => product.ProductType).FirstOrDefaultAsync(product => product.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
    {
        return await _context.Products.Include(product => product.ProductBrand).Include(product => product.ProductType).ToListAsync();
    }

    public async Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync()
    {

        return await _context.ProductBrands.ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync()
    {
        return await _context.ProductTypes.ToListAsync();
    }
}