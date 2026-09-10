using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using Ordering.Application.Abstractions;
using Ordering.Application.Mapper;
using Ordering.Application.Orders.CreateOrder;

namespace Ordering.Application.EventBusConsumer;

public class BasketOrderingConsumer(
    ICommandHandler<CreateOrderCommand, int> mediator,
    ILogger<BasketOrderingConsumer> logger) : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        using var scope = logger.BeginScope($"Consuing basket checkout Event for Id{context.Message.CorrelationId}");
        var command = context.Message.ToCheckoutOrderCommand();
        var result = await mediator.Handle(command,context.CancellationToken);
        logger.LogInformation("consumed successfully");
    }
}