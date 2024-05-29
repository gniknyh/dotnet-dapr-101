namespace Link.Mydapr.Service.Basket.Model
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