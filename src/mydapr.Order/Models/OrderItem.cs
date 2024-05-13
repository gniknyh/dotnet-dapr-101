namespace Link.Mydapr.Service.Ordering.Models
{
    public class OrderItem
    {
        public Guid Id {get; set;}

        public int ProductId {get; set;}

        // public Guid OrderId {get; set;}

        public int Unit {get; set;}
    }
}