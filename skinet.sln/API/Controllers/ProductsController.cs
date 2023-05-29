﻿using API.DTOs;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
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
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProductsAsync(string sort)
    {
        var spec = new ProductWithTypeAndBrandSpecification(sort);
        var products = await _productRepository.ListAsync(spec);
        var productsDto = _mapper.Map<
            IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
        return Ok(productsDto);
    }

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
