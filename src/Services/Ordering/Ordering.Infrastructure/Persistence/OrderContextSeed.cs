using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{

    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
        {
            new Order() 
            {
                UserName = "bufa",
                FirstName = "Douglas",
                LastName = "Souto", 
                EmailAddress = "douglas.bufaonanet@outlook.com",
                AddressLine = "Guanhães", 
                Country = "Brazil/MG", 
                TotalPrice = 350 
            }
        };
    }
}