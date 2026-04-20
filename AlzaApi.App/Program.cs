using AlzaApi.DAL;
using AlzaApi.DAL.Interfaces;
using AlzaApi.DAL.Repositories;
using AlzaApi.DAL.Seeds;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IProductRepository, ProductRepository>();


var app = builder.Build();

// Just for this project, the swagger would be always on
// In a real project, you might want to conditionally enable it based on the environment
app.UseSwagger();
app.UseSwaggerUI();

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
