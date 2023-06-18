using API.DTOs;
using AutoMapper;
using Core.Entities.OrderAggregate;

namespace API.Helpers;

public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
{
    private readonly IConfiguration _config;

    public OrderItemUrlResolver(IConfiguration config)
    {
        _config = config;
    }

    public string Resolve(
        OrderItem source, 
        OrderItemDTO destination, 
        string destMember, 
        ResolutionContext context)
    {
        return !string.IsNullOrEmpty(source.ItemOrdered.PictureUrl) ? 
            _config["ApiUrl"] + source.ItemOrdered.PictureUrl : null;
    }
}
