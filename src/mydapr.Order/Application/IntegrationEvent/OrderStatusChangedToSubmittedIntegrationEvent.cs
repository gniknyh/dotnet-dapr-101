namespace Link.Mydapr.Service.Order.IntegrationEvent
{
    public record OrderStatusChangedToSubmittedIntegrationEvent
    (
        int CustomerId
    );
}