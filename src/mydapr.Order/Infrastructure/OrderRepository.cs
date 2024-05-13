using Link.Mydapr.Service.Ordering.Models;
using Microsoft.EntityFrameworkCore;

namespace Link.Mydapr.Service.Ordering.Infrastructure
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository(OrderingDbContext orderingDbContext)
        {
            _orderDbContext = orderingDbContext;
        }

        public async Task<Order?> AddOrderAsync(Order order)
        {
            _orderDbContext.Add(order);
            await _orderDbContext.SaveChangesAsync();

            return await GetOrderByIdAsync(order.Id);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _orderDbContext.Orders.Where(o => o.Id == id)
            .Include(o => o.OrderItems)
            .SingleOrDefaultAsync();
        }

        public Task UpdateOrderAsync(Order order)
        {
            _orderDbContext.Update(order);
            return _orderDbContext.SaveChangesAsync();
        }

        private readonly OrderingDbContext _orderDbContext;
    }
}