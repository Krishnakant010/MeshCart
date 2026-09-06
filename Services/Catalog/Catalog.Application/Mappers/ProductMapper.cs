using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Application.Mappers;

public static class ProductMapper
{
    public static ProductResponse ToResponse(this Product product)
    {
        if (product == null) return null;
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Summary = product.Summary,
            Brand = product.Brand,
            CreatedDate = product.CreatedDate,
            ImageFile = product.ImageFile,
            Description = product.Description,
            Price = product.Price,
            Type = product.Type
        };
    }
    public static IList<ProductResponse> ToResponseList(this IEnumerable<Product> products)
    {
        return products.Select(b => b.ToResponse()).ToList();
    }
    public static Product ToEntity(this CreateProductCommand command, ProductBrand brand, ProductType type)
    {
        return new Product
        {
            Price = command.Price,
            Brand = brand,
            Name = command.Name,
            Summary = command.Summary,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Type = type,
            CreatedDate = DateTimeOffset.UtcNow
        };
    }
    public static Pagination<ProductResponse> ToResponse(this Pagination<Product> pagination)
        => new Pagination<ProductResponse>(
            pagination.PageIndex,
            pagination.PageSize,
            pagination.Count,
            pagination.Data.Select(p=>p.ToResponse()).ToList()
            );
    
    
    public static Product ToUpdateEntity(this UpdateProductCommand command,Product product, ProductBrand brand, ProductType productType)
    {
        return new Product
        {
            Id = product.Id,
            Name = command.Name,
            Brand = brand,
            Type = productType,
            Summary = command.Summary,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
            CreatedDate = product.CreatedDate
        };
    }

    public static ProductDto ToDto(this ProductResponse productResponse)
    {
        if (productResponse == null) return null;
        return new ProductDto(
            productResponse.Id,
            productResponse.Name,
            productResponse.Summary,
            productResponse.Description,
            productResponse.ImageFile,
            productResponse.Brand.ToDto(),
            productResponse.Type.ToDto(),
            productResponse.Price,
            productResponse.CreatedDate
        );
    }

    public static UpdateProductCommand ToUpdateProductCommand(this UpdateProductDto productDto, string id)
    {
        return new UpdateProductCommand
        {
            BrandId = productDto.BrandId,
            Description = productDto.Description,
            Id = id,
            ImageFile = productDto.ImageFile,
            Name = productDto.Name,
            Price = productDto.Price,
            TypeId = productDto.TypeId
        };
    }
    public static CreateProductCommand ToCreateProductCommand(this CreateProductDto productDto)
    {
        return new CreateProductCommand
        {
            BrandId = productDto.BrandId,
            Description = productDto.Description,
            ImageFile = productDto.ImageFile,
            Name = productDto.Name,
            Price = productDto.Price,
            TypeId = productDto.TypeId
        };
    }
}