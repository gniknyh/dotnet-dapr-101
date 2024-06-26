using Link.Mydapr.Util.Pubsub;

namespace Link.Mydapr.Service.Basket.Events
{
    public record OrderStatusChangedToSubmittedIntegrationEvent
    (
        int CustomerId
    ) : IntegrationEvent;
}