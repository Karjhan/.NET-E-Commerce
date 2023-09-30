using Backend_API.DataContexts;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add repositories and services as scoped
// builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add AutoMapper service for automatic mapping (ex: entity -> DTO)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add entity dbContext for app, add sqlite connection for dbContext
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure app to serve static files (default: wwwroot)
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Auto applying new migrations at build+run, or creating the DB otherwise + initial seeding
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception e)
{
    logger.LogError(e, "An error occured during migration!");
}

app.Run();