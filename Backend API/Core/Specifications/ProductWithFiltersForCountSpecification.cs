using Backend_API.Entities;

namespace Core.Specifications;

public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
{
    public ProductWithFiltersForCountSpecification(ProductSpecificationParams productParams) : base(product => 
        (string.IsNullOrEmpty(productParams.Search) || product.Name.ToLower().Contains(productParams.Search)) &&
        (!productParams.BrandId.HasValue || product.ProductBrandId == productParams.BrandId) && 
        (!productParams.TypeId.HasValue || product.ProductTypeId == productParams.TypeId))
    {
        
    }
}