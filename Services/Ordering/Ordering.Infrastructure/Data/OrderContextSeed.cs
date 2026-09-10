using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation($"Ordering Database: {typeof(OrderContext).Name} seeded");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            { 
                new()
                { 
                    UserName = "krishna.g",
                    FirstName = "krishnakant",
                    LastName = "Gangurde",
                    EmailAddress = "krishgangurde@ecommerce.net",
                    AddressLine = "Mumbai",
                    State = "MH",
                    Country = "India",
                    ZipCode = "421311",

                    CardName = "Visa",
                    CardNumber = "4111111111111111",
                    CreatedBy = "Krish",
                    Expiration = "12/25",
                    Cvv = "123",
                    PaymentMethod = 1,
                    LastEditedBy = "Krish",
                    EditedOn = DateTime.UtcNow
                }
            };
        }
    }
}