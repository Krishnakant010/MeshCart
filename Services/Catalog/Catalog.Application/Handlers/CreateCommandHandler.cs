using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class CreateCommandHandler(IProductRepository productRepository):IRequestHandler<CreateProductCommand,ProductResponse>
{
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var brand = await productRepository.GetBrandByIdAsync(request.BrandId);
        var type = await productRepository.GetTypeByIdAsync(request.TypeId);

        if (brand == null || type == null) throw new ApplicationException("Invalid Brand or ProductType Specified");

        var productEntity = request.ToEntity(brand,type);
        var newProduct =  await productRepository.CreatProductAsync(productEntity);
        return newProduct.ToResponse();
    }
}