using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Params;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseController
{
    private readonly IBulkRepository<Product> _productRepository;
    private readonly IBulkRepository<ProductBrand> _productBrandRepository;
    private readonly IBulkRepository<ProductType> _productTypeRepository;
    private readonly IMapper _mapper;

    public ProductsController(
        IBulkRepository<Product> productRepository,
        IBulkRepository<ProductBrand> productBrandRepository,
        IBulkRepository<ProductType> productTypeRepository,
        IMapper mapper
        )
    {
        _productRepository = productRepository;
        _productBrandRepository = productBrandRepository;
        _productTypeRepository = productTypeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Cached(600)]
    public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProductsAsync(
        [FromQuery] ProductSpecParams productParams)
    {
        var spec = new ProductWithTypeAndBrandSpecification(productParams);
        var countSpec = new ProductWithFilterForCountSpecification(productParams);
        var totalItems = await _productRepository.CountAsync(countSpec);
        var products = await _productRepository.ListAsync(spec);
        var productsDto = _mapper.Map<
            IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
        var value = new Pagination<ProductToReturnDTO>
        {
            PageIndex = productParams.PageIndex,
            PageSize = productParams.PageSize,
            Count = totalItems,
            Data = productsDto
        };
        return Ok(value);
    }

    [Cached(600)]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDTO>> GetProductByIdAsync(int id)
    {
        var spec = new ProductWithTypeAndBrandSpecification(id);

        var product = await _productRepository.GetEntityWithSpec(spec);
        if (product != null)
        {
            var productDto = _mapper.Map<Product, ProductToReturnDTO>(product);
            return Ok(productDto);
        }

        ApiResponse apiResponse = new(404, $"Product with Id: {id} not found!");
        return base.NotFound(apiResponse);
    }

    [Cached(600)]
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrandsAsync()
    {
        return Ok(await _productBrandRepository.ListAllAsync());
    }

    [Cached(600)]
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypesAsync()
    {
        return Ok(await _productTypeRepository.ListAllAsync());
    }
}
