using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Handlers;

public class BasketCheckoutHandler(IMediator mediator,IPublishEndpoint publishEndpoint,ILogger<BasketCheckoutHandler>logger):IRequestHandler<BasketCheckoutCommand ,Unit>
{
    public async Task<Unit> Handle(BasketCheckoutCommand request, CancellationToken cancellationToken)
    {
        var basketCheckoutDto = request.Dto;
        var basketResponse = await mediator.Send(new GetBasketByUserNameQuery(basketCheckoutDto.UserName));
        if (basketResponse is null || !basketResponse.Items.Any())
            throw new InvalidOperationException("Basket not found or empty");

        var basket = basketResponse.ToEntity();
        // map
        var evt = basketCheckoutDto.ToBasketCheckoutEvent(basket);
        logger.LogInformation($"Publishing basket checkout event for user {request.Dto.UserName}");
        await publishEndpoint.Publish(evt);
        await mediator.Send(new DeleteBasketByUserNameCommand(basketCheckoutDto.UserName), cancellationToken);
        return Unit.Value;
    }
} 