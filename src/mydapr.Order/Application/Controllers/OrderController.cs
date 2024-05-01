

using Dapr.Client;
using Link.Mydapr.Service.Order.IntegrationEvent;
using Link.Mydapr.Service.Order.Models;
using Microsoft.AspNetCore.Mvc;

namespace Link.Mydapr.Service.Order.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController  : ControllerBase
    {
        public OrderController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        [HttpPost("submit")]
        public async Task<ActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            await _daprClient.PublishEventAsync(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent),
            new OrderStatusChangedToSubmittedIntegrationEvent(createOrderDto.customerId));
            return Ok();
        }

        private readonly DaprClient _daprClient;
        private const string DAPR_PUBSUB_NAME = "mydapr-pubsub";

    }
}
