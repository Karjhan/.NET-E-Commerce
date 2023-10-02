using System.Text;
using Backend_API.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend_API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppIdentityDBContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("IdentityConnection"));
        });
        services.AddIdentityCore<AppUser>(options =>
        {
            // add identity options here
            
        }).AddEntityFrameworkStores<AppIdentityDBContext>().AddSignInManager<SignInManager<AppUser>>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
                ValidIssuer = configuration["Token:Issuer"],
                ValidateIssuer = true,
                ValidateAudience = false
            };
        });
        services.AddAuthorization();
        return services;
    }
}