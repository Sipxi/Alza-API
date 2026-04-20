using AlzaApi.App.Middleware;
using AlzaApi.BL.Interfaces;
using AlzaApi.BL.Services;
using AlzaApi.DAL;
using AlzaApi.DAL.Interfaces;
using AlzaApi.DAL.Repositories;
using AlzaApi.DAL.Seeds;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new() { Title = "Alza API", Version = "v1", Description = "Classic API" });
    c.SwaggerDoc("v2",
        new() { Title = "Alza API", Version = "v2", Description = "Pagination API" });

    c.DocInclusionPredicate((docName, apiDesc) => apiDesc.GroupName == docName);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// Just for this project, the swagger would be always on
// In a real project, you might want to conditionally enable it based on the environment
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 - Classic");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 - Pagination");
});

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Development environment");
    await DbInitializer.InitializeAsync(context);
}
else
{
    Console.WriteLine("Production environment");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
