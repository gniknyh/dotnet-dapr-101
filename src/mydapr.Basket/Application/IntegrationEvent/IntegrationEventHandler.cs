using Dapr;
using Microsoft.AspNetCore.Mvc;

namespace Link.Mydapr.Service.Basket.IntegrationEvent
{
    public class IntegrationEventHandler
    {
        private const string DAPR_PUBSUB_NAME = "mydapr-pubsub";


        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent))]
        public Task HandleAsync(
            OrderStatusChangedToSubmittedIntegrationEvent @event,
            [FromServices] OrderStatusChangedToSubmittedIntegrationEventHandler handler)

        => handler.Handle(@event);
    }
}