using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetPreconfiguredOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order 
                { 
                    UserName = "swn", 
                    FirstName = "Freddie", 
                    LastName = "Amoako", 
                    EmailAddress = "freddie@example.com",
                    AddressLine = "Brentwood", 
                    Country = "USA",
                    TotalPrice = 2264M,
                    CardName="Visa",
                    CardNumber="56566556665565", 
                    Expiration="555",
                    CVV = "565", 
                    LastModifiedBy="freddie",
                    State = "CA",
                    ZipCode ="566665"
                }
            };
        }
    }
}
