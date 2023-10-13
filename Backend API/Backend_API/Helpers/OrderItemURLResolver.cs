using AutoMapper;
using Backend_API.DTO;
using Backend_API.Entities.OrderAggregate;

namespace Backend_API.Helpers;

public class OrderItemURLResolver : IValueResolver<OrderItem, OrderItemDTO, string>
{
    private IConfiguration _configuration { get; }
        
    public OrderItemURLResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string? Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.ItemOrdered.PictureURL))
        {
            return _configuration["ApiURL"] + source.ItemOrdered.PictureURL;
        }

        return null;
    }
}