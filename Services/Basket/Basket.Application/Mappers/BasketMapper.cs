using Basket.Application.Commands;
using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mappers;

public static class BasketMapper
{
    public static ShoppingCartResponse ToResponse(this ShoppingCart shoppingCart)
    {
        return new ShoppingCartResponse
        {
            UserName = shoppingCart.UserName,
            Items = shoppingCart.Items.Select(i => new ShoppingCartItemResponse()
            {
                Quantity = i.Quantity,
                ImageFile = i.ImageFile,
                ProductId = i.ProductId,
                Price = i.Price,
                ProductName = i.ProductName
            }).ToList()
        };
    }

    public static readonly Func<ShoppingCart, ShoppingCartResponse> MapCart =
        cart => new ShoppingCartResponse
        {
            UserName = cart.UserName, Items = cart.Items.Select(i => new ShoppingCartItemResponse()
            {
                Quantity = i.Quantity,
                ImageFile = i.ImageFile,
                ProductId = i.ProductId,
                Price = i.Price,
                ProductName = i.ProductName
            }).ToList()
        };


    public static ShoppingCart ToEntity(this CreateShoppingCartCommand cartCommand)
    {
        return new ShoppingCart
        {
            Items = cartCommand.items.Select(i => new ShoppingCartItem
            {
                ImageFile = i.ImageFile,
                Price = i.Price,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity
            }).ToList(),
            UserName = cartCommand.UserName
        };
    }
}