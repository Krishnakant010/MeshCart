using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductsByBrandHandler(IProductRepository productRepository):IRequestHandler<GetProductsByBrandQuery,IList<ProductResponse>>
{
    public async Task<IList<ProductResponse>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetProductsByBrandAsync(brandName: request.BrandName);
        return products.ToResponseList();
    }
}