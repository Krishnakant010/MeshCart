using EventBus.Messages.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Dispatcher;

public class OutboxMessageDispatcher(IServiceProvider serviceProvider, ILogger<OutboxMessageDispatcher> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderContext>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            var pendingMessages = await db.OutboxMessages.Where(x => x.ProcessedOn == null).OrderBy(x => x.OccurredOn)
                .Take(20).ToListAsync(cancellationToken: stoppingToken);
            foreach (var message in pendingMessages)
            {
                try
                {
                    var orderCreatedEvent = JsonConvert.DeserializeObject<OrderCreatedEvent>(message.Content);
                    await publishEndpoint.Publish(orderCreatedEvent);
                    message.ProcessedOn = DateTime.UtcNow;
                    logger.LogInformation($"Published outbox message {message.Id}");
                }
                catch (Exception e)
                {
                    logger.LogError(e,$"Error while sendng  msg {message.Id}");
                }

                await db.SaveChangesAsync();
                await Task.Delay(6000, stoppingToken);
            }
        }
    }
}