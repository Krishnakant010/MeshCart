using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController(IMediator mediator) : ControllerBase
{
    [HttpGet("GetAllProducts")]
    public async Task<ActionResult<IList<ProductDto>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
    {
        try
        {
            var query = new GetAllProductsQuery(catalogSpecParams);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal server error",
                error = ex.Message
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(string id)
    {
        try
        {
            var query = new GetProductByIdQuery(id);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal server error",
                error = ex.Message
            });
        }
    }

    [HttpGet("productName/{productName}")]
    public async Task<ActionResult<IList<ProductDto>>> GetProductByName(string productName)
    {
        try
        {
            var query = new GetProductByNameQuery(productName);
            var result = await mediator.Send(query);
            if (result == null || !result.Any()) return NotFound();
            var dtoList = result.Select(p => p.ToDto()).ToList();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal server error",
                error = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        try
        {
            var createProductCommand = createProductDto.ToCreateProductCommand();
            var result = await mediator.Send(createProductCommand);
            return result.ToDto();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal server error",
                error = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        try
        {
            var command = new DeleteProductByIdCommand(id);
            var result = await mediator.Send(command);
            if (!result) return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal server error",
                error = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, UpdateProductDto updateProductDto)
    {
        try
        {
            var command = updateProductDto.ToUpdateProductCommand(id);
            var result = await mediator.Send(command);
            if (!result) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal server error",
                error = ex.Message
            });
        }
    }

    [HttpGet("GetAllBRands")]
    public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
    {
        var query =new  GetAllBrandsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("GetAllTypes")]
    public async Task<ActionResult<IEnumerable<TypesResponse>>> GetTypes()
    {
        var query =new  GetAllTypesQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("/brand/{brand}",Name = "GetProductsByBrandName")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBrand(string brand)
    {
        var query =new  GetProductsByBrandQuery(brand);
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
