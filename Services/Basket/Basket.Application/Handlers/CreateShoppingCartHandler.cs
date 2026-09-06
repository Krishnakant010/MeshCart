using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartHandler(IBasketRepository basketRepository):IRequestHandler<CreateShoppingCartCommand,ShoppingCartResponse>
{
    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        var shoppingCartEntity = request.ToEntity();
        var updatedCart = await basketRepository.UpsertBasket(shoppingCartEntity);

        return updatedCart.ToResponse();
    }
}