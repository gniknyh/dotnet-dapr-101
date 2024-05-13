using Link.Mydapr.Util.Pubsub;

namespace Link.Mydapr.Service.Ordering.Events
{
    public record OrderStatusChangedToSubmittedIntegrationEvent
    (
        int CustomerId
    ): IntegrationEvent;
}