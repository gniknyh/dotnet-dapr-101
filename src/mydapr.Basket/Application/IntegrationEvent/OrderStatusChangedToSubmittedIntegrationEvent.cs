namespace Link.Mydapr.Service.Basket.IntegrationEvent
{
    public record OrderStatusChangedToSubmittedIntegrationEvent
    (
        Guid OrderId,
        int CustomerId
    );
}