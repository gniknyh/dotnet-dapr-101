namespace Link.Mydapr.Service.Ordering.Models
{
    public enum OrderStatus
    {
        Submitted,
        AwaitingStockValidation,
        StockValidated,
        Paid,
        Shipped,
        Cancelled
    }
}