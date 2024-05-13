namespace Link.Mydapr.Service.Ordering.Models
{
    public class Order
    {
        public Guid Id {get; set;}

        public DateTime CreatedTime {get; set;}

        public int CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public void AddOrderItem(int productId, int unit)
        {
            var orderItem = new OrderItem{
                ProductId = productId,
                Unit = unit
            };
            _orderItems.Add(orderItem);
        }


        private List<OrderItem> _orderItems = new();

    }
}