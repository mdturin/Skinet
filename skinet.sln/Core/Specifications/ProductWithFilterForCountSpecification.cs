using Core.Entities;
using Core.Params;
using Core.Services;

namespace Core.Specifications;

public class ProductWithFilterForCountSpecification : BaseSpecification<Product>
{
    public ProductWithFilterForCountSpecification(ProductSpecParams productParams)
        : base(SpecificationService.BuildSpecification(productParams)){}
}
