using Catalog.Core.Entities;

namespace Catalog.Application.Responses;

public record ProductResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Summary { get; init; }
    public string ImageFile { get; init; }
    public string Description { get; init; }
    public ProductBrand Brand { get; init; }
    public ProductType Type { get; init; }
    public Decimal Price { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
    
};