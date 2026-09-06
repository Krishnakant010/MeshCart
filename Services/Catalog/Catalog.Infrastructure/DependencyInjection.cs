using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        return services;
    }
}