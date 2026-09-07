using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartHandler(IBasketRepository basketRepository,DiscountGrpcService service):IRequestHandler<CreateShoppingCartCommand,ShoppingCartResponse>
{
    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        
        foreach(var item in request.items)
        {
            var coupon = await service.GetDiscount(item.ProductName);
            item.Price -= (decimal)coupon.Amount;
        }
        
        var shoppingCartEntity = request.ToEntity();
        var updatedCart = await basketRepository.UpsertBasket(shoppingCartEntity);
        
        return updatedCart.ToResponse();
    }
}