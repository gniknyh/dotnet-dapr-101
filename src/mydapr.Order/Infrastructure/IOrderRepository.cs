using Link.Mydapr.Service.Ordering.Models;

namespace Link.Mydapr.Service.Ordering.Infrastructure
{
    public interface IOrderRepository
    {
        public Task<Order?> AddOrderAsync(Order order);

        public Task UpdateOrderAsync(Order order);

        public Task<Order?> GetOrderByIdAsync(Guid id);
    }
}