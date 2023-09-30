using Backend_API.DataContexts;
using Backend_API.Extensions;
using Backend_API.Middlewares;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
// Exception Middleware has to go at the top of HTTP pipeline
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

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