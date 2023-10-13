using AutoMapper;
using Backend_API.DTO;
using Backend_API.Entities;
using Backend_API.Entities.OrderAggregate;
using Address = Backend_API.Entities.Identity.Address;

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
        CreateMap<AddressDTO, Entities.OrderAggregate.Address>();
        CreateMap<Order, OrderToReturnDTO>()
            .ForMember(destMember => destMember.DeliveryMethod, options => options.MapFrom(source => source.DeliveryMethod.ShortName))
            .ForMember(destMember => destMember.ShippingPrice, options => options.MapFrom(source => source.DeliveryMethod.Price));
        CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(desetMember => desetMember.ProductId, options => options.MapFrom(source => source.ItemOrdered.ProductItemId))
            .ForMember(desetMember => desetMember.ProductName, options => options.MapFrom(source => source.ItemOrdered.ProductName))
            .ForMember(desetMember => desetMember.ProductURL, options => options.MapFrom<OrderItemURLResolver>());
    }
}