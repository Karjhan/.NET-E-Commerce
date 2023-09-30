using System.Linq.Expressions;
using Backend_API.Entities;

namespace Core.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(ProductSpecificationParams productParams) : base(product => 
        (string.IsNullOrEmpty(productParams.Search) || product.Name.ToLower().Contains(productParams.Search)) &&
        (!productParams.BrandId.HasValue || product.ProductBrandId == productParams.BrandId) && 
        (!productParams.TypeId.HasValue || product.ProductTypeId == productParams.TypeId))
    {
        AddInclude(product => product.ProductType);
        AddInclude(product => product.ProductBrand);
        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);
        if (!string.IsNullOrEmpty(productParams.Sort))
        {
            switch (productParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(product => product.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(product => product.Price);
                    break;
                default:
                    AddOrderBy(product => product.Name);
                    break;
            }
        }
    }

    public ProductsWithTypesAndBrandsSpecification(int id) : base(product => product.Id == id)
    {
        AddInclude(product => product.ProductType);
        AddInclude(product => product.ProductBrand);
    }
}