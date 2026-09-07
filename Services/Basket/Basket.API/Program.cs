using System.Reflection;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Application.Settings;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Settings;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddEndpointsApiExplorer();
//swaggger

var assemblies = new Assembly[]
{
    Assembly.GetEntryAssembly(), typeof(CreateShoppingCartHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection("CacheSettings"));

builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection("CacheSettings"));
builder.Services.Configure<GrpcSettings>(
    builder.Configuration.GetSection("GrpcSettings"));

// Iption registio
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>((sp, cfg) =>
{
    var settings = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
    cfg.Address = new Uri(settings.DiscountUrl);
});

//grpc service
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>((sp, cfg) =>
{
    var settings = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
    cfg.Address = new Uri(settings.DiscountUrl);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration["CacheSettings:ConnectionString"];
});
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();