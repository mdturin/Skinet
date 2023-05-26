using Core.Entities;

namespace Core.Specifications;

public class ProductWithTypeAndBrandSpecification : BaseSpecification<Product>
{
    public ProductWithTypeAndBrandSpecification()
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
    }

    public ProductWithTypeAndBrandSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
    }
}
