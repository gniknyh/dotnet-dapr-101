namespace Link.Mydapr.Service.Ordering.Events
{
    public class CustomerBasket
    {
        public int CustomerId {get; set;}

        public List<BasketItem> Items {get; set;}

        public CustomerBasket(int customerId)
        {
            CustomerId = customerId;
            Items = new();
        }
    }
}