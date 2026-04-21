using AlzaApi.App.Middleware;
using AlzaApi.BL.Interfaces;
using AlzaApi.BL.Services;
using AlzaApi.DAL;
using AlzaApi.DAL.Interfaces;
using AlzaApi.DAL.Repositories;
using AlzaApi.DAL.Seeds;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var useMock = builder.Configuration.GetValue<bool>("UseMockData");

// Controllers & API explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Alza API", Version = "v1", Description = "Classic API" });
    c.SwaggerDoc("v2",
        new() { Title = "Alza API", Version = "v2", Description = "Pagination API" });
    c.DocInclusionPredicate((docName, apiDesc) => apiDesc.GroupName == docName);
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "AlzaApi.App.xml"));
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "AlzaApi.Common.xml"));
});

// Data layer
if (useMock)
{
    builder.Services.AddScoped<IProductRepository, MockProductRepository>();
}
else
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                              ?? throw new InvalidOperationException(
                                  "Connection string 'DefaultConnection' not found.");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddScoped<IProductRepository, ProductRepository>();
}

// Business layer
builder.Services.AddScoped<IProductService, ProductService>();

// Error handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Database initialization
if (!useMock)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (app.Environment.IsDevelopment())
        await DbInitializer.InitializeAsync(context);
}

// Middleware pipeline
app.UseExceptionHandler();

// Note: Swagger is always enabled in this project for simplicity.
// In production, consider restricting it to Development environment only.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 - Classic");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 - Pagination");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
