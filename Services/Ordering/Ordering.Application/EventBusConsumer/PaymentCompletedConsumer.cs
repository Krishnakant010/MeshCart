using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Core.Repositories;

namespace Ordering.Application.EventBusConsumer;

public class PaymentCompletedConsumer(IOrderRepository orderRepository,ILogger<PaymentCompletedConsumer> logger):IConsumer<PaymentCompletedEvent>
{
    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        var order = await orderRepository.GetByIdAsync(context.Message.OrderId);
        if(order==null) logger.LogWarning($"Order not found for Id{context.Message.OrderId} and correlationId : {context.CorrelationId}");

        order.Status = Core.Entities.OrderStatus.Paid;
        await orderRepository.UpdateAsync(order);
        logger.LogInformation($"Completed payment for Order id {order.Id}");
    }
}