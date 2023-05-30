using Core.Entities;
using Core.Params;
using System.Linq.Expressions;

namespace Core.Services;

public class SpecificationService
{
    public static Expression<Func<Product, bool>> BuildSpecification(ProductSpecParams productParams)
    {
        var parameterExpr = Expression.Parameter(typeof(Product), "p");

        var namePropertyExpr = Expression.Property(parameterExpr, "Name");
        var searchExpr = Expression.Constant(productParams.Search);

        // Filter by product name if search parameter is provided
        var filterByName = string.IsNullOrEmpty(productParams.Search)
            ? (Expression)Expression.Constant(true)
            : Expression.Call(
                Expression.Call(namePropertyExpr, "ToLower", null),
                "Contains",
                null,
                searchExpr
            );

        // Filter by brand ID if provided
        var filterByBrandId = !productParams.BrandId.HasValue
            ? (Expression)Expression.Constant(true)
            : Expression.Equal(
                Expression.Property(parameterExpr, "ProductBrandId"),
                Expression.Constant(productParams.BrandId.Value)
            );


        // Filter by type ID if provided
        var filterByTypeId = !productParams.TypeId.HasValue
            ? (Expression)Expression.Constant(true)
            : Expression.Equal(
                Expression.Property(parameterExpr, "ProductTypeId"),
                Expression.Constant(productParams.TypeId)
            );

        var finalFilterExpression = Expression.AndAlso(
            Expression.AndAlso(filterByName, filterByBrandId),
            filterByTypeId
        );

        return Expression.Lambda<Func<Product, bool>>(finalFilterExpression, parameterExpr);
    }
}
