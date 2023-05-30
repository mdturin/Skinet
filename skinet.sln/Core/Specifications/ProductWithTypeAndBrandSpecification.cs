using Core.Entities;
using Core.Params;
using Core.Services;

namespace Core.Specifications;

public class ProductWithTypeAndBrandSpecification : BaseSpecification<Product>
{
    public ProductWithTypeAndBrandSpecification(ProductSpecParams productParams)
        : base(SpecificationService.BuildSpecification(productParams))
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
        AddOrderBy(x => x.Name);
        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

        switch(productParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(p => p.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(p => p.Price);
                break;
        }
    }

    public ProductWithTypeAndBrandSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
    }
}
