using System.Linq.Expressions;
using Backend_API.Entities;

namespace Core.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification()
    {
        AddInclude(product => product.ProductType);
        AddInclude(product => product.ProductBrand);
    }

    public ProductsWithTypesAndBrandsSpecification(int id) : base(product => product.Id == id)
    {
        AddInclude(product => product.ProductType);
        AddInclude(product => product.ProductBrand);
    }
}