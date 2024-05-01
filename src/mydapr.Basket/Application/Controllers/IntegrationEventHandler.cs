using Dapr;
using Link.Mydapr.Service.Basket.IntegrationEvent;
using Microsoft.AspNetCore.Mvc;

namespace Link.Mydapr.Service.Basket.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IntegrationEventHandler : ControllerBase
    {
        private const string DAPR_PUBSUB_NAME = "mydapr-pubsub";


        [HttpPost("OrderStatusChangedToSubmitted")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent))]
        public Task HandleAsync(
            OrderStatusChangedToSubmittedIntegrationEvent @event,
            [FromServices] OrderStatusChangedToSubmittedIntegrationEventHandler handler)
            => handler.Handle(@event);
            
    }
}