using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository(OrderContext context) :BaseRepository<Order>(context),IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    {
        return await context.Orders.AsNoTracking().Where(i => i.UserName.Equals(userName.ToLower())).ToListAsync();
    }

    public async Task AddOutBoxMessageAsync(OutboxMessage outboxMessage)
    {
         await context.OutboxMessages.AddAsync(outboxMessage);
         await context.SaveChangesAsync();
         
    }
}