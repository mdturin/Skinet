using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

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

        CreateMap<Address, AddressDTO>().ReverseMap();
    }
}
