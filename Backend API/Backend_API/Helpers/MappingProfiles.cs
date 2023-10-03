using AutoMapper;
using Backend_API.DTO;
using Backend_API.Entities;
using Backend_API.Entities.Identity;

namespace Backend_API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(destMember => destMember.ProductBrand, options => options.MapFrom(source => source.ProductBrand.Name))
            .ForMember(destMember => destMember.ProductType, options => options.MapFrom(source => source.ProductType.Name))
            .ForMember(destMemeber => destMemeber.PictureURL, options => options.MapFrom<ProductURLResolver>());
        CreateMap<Address, AddressDTO>().ReverseMap();
        CreateMap<CustomerBasketDTO, CustomerBasket>();
        CreateMap<BasketItemDTO, BasketItem>();
    }
}