using AutoMapper;
using Backend_API.DTO;
using Backend_API.Entities;
using IConfigurationProvider = Microsoft.Extensions.Configuration.IConfigurationProvider;

namespace Backend_API.Helpers;

public class ProductURLResolver : IValueResolver<Product, ProductDTO, string>
{
    private IConfiguration _configuration { get; }
    
    public ProductURLResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string? Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureURL))
        {
            return _configuration["ApiURL"] + source.PictureURL;
        }
        return null;
    }
}