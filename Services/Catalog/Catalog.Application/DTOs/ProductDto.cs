namespace Catalog.Application.DTOs;

public record ProductDto(string Id,string Name,string Summary,string Description,string ImageFile,BrandDto brand,TypeDto TypeDto,
decimal Price,DateTimeOffset CreatedDate
);

