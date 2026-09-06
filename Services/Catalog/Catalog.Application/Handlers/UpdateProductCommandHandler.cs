using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class UpdateProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<UpdateProductCommand, bool>
{
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetProductByIdAsync(request.Id);
        if (product == null) throw new KeyNotFoundException($"Product with Id not {request.Id} not found");
        // brand and type
        var brand = await productRepository.GetBrandByIdAsync(request.BrandId);
        var productType = await productRepository.GetTypeByIdAsync(request.TypeId);
        if (brand == null || productType == null) throw new ApplicationException("Invalid Brand or ProductType Specified");
    
        // mapper 
        var updatedProduct = request.ToUpdateEntity(product, brand, productType);
        var result =await productRepository.UpdateProductAsync(updatedProduct);
        return result;
    }
}