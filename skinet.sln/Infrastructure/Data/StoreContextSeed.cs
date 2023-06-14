using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                foreach(var brand in brands)
                {
                    await context.ProductBrands.AddAsync(brand);
                }
                await context.SaveChangesAsync();
            }

            if (!context.ProductTypes.Any())
            {
                var productTypesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypesData);
                foreach (var productType in productTypes)
                {
                    await context.ProductTypes.AddAsync(productType);
                }
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                foreach (var product in products)
                {
                    await context.Products.AddAsync(product);
                }
                await context.SaveChangesAsync();
            }

            if(!context.DeliveryMethods.Any())
            {
                var dmData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                foreach(var method in methods)
                {
                    await context.DeliveryMethods.AddAsync(method);
                }
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<StoreContextSeed>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}
