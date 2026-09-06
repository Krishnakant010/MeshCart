using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.DTOs;

public record class CreateProductDto
{
    [Required] public string Name { get; init; }
    [Required] public string Summary { get; init; }
    [Required] public string Description { get; init; }
    [Required] public string ImageFile { get; init; }
    [Required] public string BrandId { get; init; }
    [Required] public string TypeId { get; init; }
    [Range(0.01,double.MaxValue,ErrorMessage = "Price must be greater than 0")] public Decimal Price { get; init; }
}
//reference type with value base symantics
public record class UpdateProductDto
{
    [Required] public string Name { get; init; }
    [Required] public string Summary { get; init; }
    [Required] public string Description { get; init; }
    [Required] public string ImageFile { get; init; }
    [Required] public string BrandId { get; init; }
    [Required] public string TypeId { get; init; }
    [Range(0.01,double.MaxValue,ErrorMessage = "Price must be greater than 0")] public Decimal Price { get; init; }
}