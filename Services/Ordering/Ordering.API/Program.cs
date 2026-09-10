using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.Extensions;
using Ordering.Application.EventBusConsumer;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Dispatcher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOrderingServices(builder.Configuration);

builder.Services.AddHostedService<OutboxMessageDispatcher>();

builder.Services.AddMassTransit((config =>
{
    config.AddConsumer<BasketOrderingConsumer>();
    config.AddConsumer<PaymentCompletedConsumer>();
    config.AddConsumer<PaymentFailedConsumer>();
    config.UsingRabbitMq((ctx, conf) =>
    {
        conf.Host((builder.Configuration["EventBusSettings:HostAddress"]));
        conf.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue,
            c => { c.ConfigureConsumer<BasketOrderingConsumer>(ctx); });
        conf.ReceiveEndpoint(EventBusConstant.PaymentCompletedQueue,
            c => { c.ConfigureConsumer<PaymentCompletedConsumer>(ctx); });
        conf.ReceiveEndpoint(EventBusConstant.PaymentFailedQueue,
            c => { c.ConfigureConsumer<PaymentFailedConsumer>(ctx); });
        conf.ReceiveEndpoint();
    });
}));
var app = builder.Build();

app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetRequiredService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();
});

app.UseSwagger();
app.UseSwaggerUI();

// No HTTPS inside the container for now
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();