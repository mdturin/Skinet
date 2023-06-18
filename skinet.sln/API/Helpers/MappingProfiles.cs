using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        //Creating map for Product to ProductToReturnDTO with properties 
        //    mapping and custom resolver for PictureUrl
        CreateMap<Product, ProductToReturnDTO>()
            .ForMember(p => p.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(p => p.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductUrlResolver>())
            ;

        CreateMap<Core.Entities.Identity.Address, AddressDTO>().ReverseMap();

        CreateMap<CustomerBasketDTO, CustomerBasket>().ReverseMap();

        CreateMap<BasketItemDTO, BasketItem>().ReverseMap();

        CreateMap<AddressDTO, Core.Entities.OrderAggregate.Address>();

        CreateMap<Order, OrderToReturnDTO>()
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
            ;

        CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>())
            ;
    }
}
