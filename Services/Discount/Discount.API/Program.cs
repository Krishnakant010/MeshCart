using System.Reflection;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;
using Discount.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(), typeof(CreateDiscountHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddGrpc();

// db
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings")
);



var app = builder.Build();

app.MigrateDatabase();
app.UseRouting();
app.UseEndpoints(end =>
{
    end.MapGrpcService<DiscountService>();
});


// Configure the HTTP request pipeline.



app.Run();