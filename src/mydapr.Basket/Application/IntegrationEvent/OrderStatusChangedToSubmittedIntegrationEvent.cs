namespace Link.Mydapr.Service.Basket.IntegrationEvent
{
    public record OrderStatusChangedToSubmittedIntegrationEvent
    (
        int CustomerId
    );
}