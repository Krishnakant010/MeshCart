using EventBus.Messages.Events;
using MassTransit;

namespace Payment.Consumer;

public class OrderCreatedConsumer(IPublishEndpoint publishEndpoint, ILogger<OrderCreatedConsumer> logger)
    : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        logger.LogInformation($"Processing payment for order id {message.Id}");
        //simulation for payment
        await Task.Delay(5000);

        if (message.TotalPrice > 0)
        {
            //simulate the success 
            var completedEvent = new PaymentCompletedEvent
            {
                OrderId = message.Id,
                CorrelationId = message.CorrelationId,
            };
            await publishEndpoint.Publish(completedEvent);
            logger.LogInformation(
                $"Payment success for OrderId {message.Id} and correlation Id {message.CorrelationId}");
        }
        else
        {
            //simulate the success 
            var failedEvent = new PaymentFailedEvent()
            {
                OrderId = message.Id,
                CorrelationId = message.CorrelationId,
                Reason = "Total Price was zero or negative."
            };
            await publishEndpoint.Publish(failedEvent);
            logger.LogWarning(
                $"Payment Failed for OrderId {message.Id} and correlation Id {message.CorrelationId}");
            
        }
    }
}