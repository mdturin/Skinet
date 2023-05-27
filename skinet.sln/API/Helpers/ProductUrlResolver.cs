﻿using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
{
    private readonly IConfiguration _config;

    public ProductUrlResolver(IConfiguration config)
    {
        _config = config;
    }

    public string Resolve(
        Product source, 
        ProductToReturnDTO destination, 
        string destMember, 
        ResolutionContext context)
    {
        var url = _config["ApiUrl"];
        if(!string.IsNullOrEmpty(source.PictureUrl))
        {
            return url + source.PictureUrl;
        }
        return null;
    }
}
