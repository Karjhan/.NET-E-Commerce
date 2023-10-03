using Microsoft.OpenApi.Models;

namespace Backend_API.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            OpenApiSecurityScheme securitySchema = new OpenApiSecurityScheme()
            {
                Description = "JWT Auth Bearer Scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                BearerFormat = "JWT"
            };
            options.AddSecurityDefinition("Bearer", securitySchema);
            OpenApiSecurityRequirement securityRequirement = new OpenApiSecurityRequirement()
            {
                {
                    securitySchema, new[] { "Bearer" }
                }
            };
            options.AddSecurityRequirement(securityRequirement);
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
    }
}