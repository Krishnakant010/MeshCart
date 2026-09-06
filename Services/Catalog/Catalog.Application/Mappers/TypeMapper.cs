using Catalog.Application.DTOs;
using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers;

public static class TypeMapper
{
    public static TypesResponse ToResponse(this ProductType brand)
    {
        return new TypesResponse
        {
            Id = brand.Id,
            Name = brand.Name
        };
    }
    public static IList<TypesResponse> ToResponseList(this IEnumerable<ProductType> brands)
    {
        return brands.Select(b => b.ToResponse()).ToList();
    }

    public static TypeDto ToDto(this ProductType productType)
    {
        return new TypeDto(productType.Id, productType.Name);
    }
}