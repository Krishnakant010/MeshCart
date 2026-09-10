using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Core.Repositories;

namespace Ordering.Application.EventBusConsumer;

public class PaymentFailedConsumer(IOrderRepository orderRepository,ILogger<PaymentFailedConsumer> logger):IConsumer<PaymentFailedEvent>
{
  
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var order = await orderRepository.GetByIdAsync(context.Message.OrderId);
        if(order==null) logger.LogWarning($"Order not found for Id{context.Message.OrderId} and correlationId : {context.CorrelationId}");

        order.Status = Core.Entities.OrderStatus.Failed;
        await orderRepository.UpdateAsync(order);
        logger.LogWarning($"Completed payment for Order id but it failed {order.Id}");
    }
}