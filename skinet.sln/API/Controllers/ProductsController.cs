using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IBulkRepository<Product> _productRepository;
    private readonly IBulkRepository<ProductBrand> _productBrandRepository;
    private readonly IBulkRepository<ProductType> _productTypeRepository;

    public ProductsController(
        IBulkRepository<Product> productRepository,
        IBulkRepository<ProductBrand> productBrandRepository,
        IBulkRepository<ProductType> productTypeRepository)
    {
        _productRepository = productRepository;
        _productBrandRepository = productBrandRepository;
        _productTypeRepository = productTypeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProductsAsync()
    {
        var spec = new ProductsWithTypesAndBrandsSpecification();
        return Ok(await _productRepository.ListAsync(spec));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null ? Ok(product) : NotFound();
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrandsAsync()
    {
        return Ok(await _productBrandRepository.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypesAsync()
    {
        return Ok(await _productTypeRepository.ListAllAsync());
    }
}
