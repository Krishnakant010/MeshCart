using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;

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

    public static BasketCheckoutEvent ToBasketCheckoutEvent(this BasketCheckoutDto dto, ShoppingCart basket)
    {
        return new BasketCheckoutEvent
        {
            UserName = dto.UserName,
            TotalPrice = basket.Items.Sum(item => item.Price * item.Quantity),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            EmailAddress = dto.EmailAddress,
            AddressLine = dto.AddressLine,
            Country = dto.Country,
            State = dto.State,
            ZipCode = dto.ZipCode,
            CardName = dto.CardName,
            CardNumber = dto.CardNumber,
            Expiration = dto.Expiration,
            Cvv = dto.Cvv,
            PaymentMethod = dto.PaymentMethod
        };
    }
    public static ShoppingCart ToEntity(this ShoppingCartResponse response)
    {
        return new ShoppingCart(response.UserName)
        {
            Items = response.Items.Select(item => new ShoppingCartItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList()
        };
    }
}