using Backend_API.DataContexts;
using Backend_API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Backend_API.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add repositories and services as scoped
        // Add Redis singleton in-memory DB service (has to survive throughout thread activity)
        services.AddSingleton<IConnectionMultiplexer>(c =>
        {
            var options = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
            return ConnectionMultiplexer.Connect(options);
        });
        // builder.Services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        // Add AutoMapper service for automatic mapping (ex: entity -> DTO)
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // Overwritting ApiController behaviour 
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();
                var errorResponse = new APIValidationErrorResponse()
                {
                    Errors = errors
                };
                return new BadRequestObjectResult(errorResponse);
            };
        });
        // Add entity dbContext for app, add sqlite connection for dbContext
        services.AddDbContext<StoreContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
        // Add CORS service for Angular client server
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(configuration["ClientURL"]);
            });
        });
        return services;
    }
}