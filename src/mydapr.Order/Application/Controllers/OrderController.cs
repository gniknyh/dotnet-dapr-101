

using Dapr;
using Dapr.Client;
using Link.Mydapr.Service.Ordering.Events;
using Link.Mydapr.Service.Ordering.Infrastructure;
using Link.Mydapr.Service.Ordering.Models;
using Link.Mydapr.Util.Pubsub;
using Microsoft.AspNetCore.Mvc;

namespace Link.Mydapr.Service.Ordering.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController  : ControllerBase
    {
        public OrderController(DaprClient daprClient,
        IOrderRepository orderRepository,
        IEventBus eventBus
        )
        {
            _daprClient = daprClient;
            _orderRepository = orderRepository;
            _eventBus = eventBus;
        }

        [HttpPost("checkout")]
        [Topic(DAPR_PUBSUB_NAME, nameof(CheckoutIntegrationEvent))]
        public async Task CheckoutCreateOrderAsync(CheckoutIntegrationEvent checkoutEvent)
        {
            var order = new Order
            {
                CreatedTime = checkoutEvent.CreatedTime,
                CustomerId = checkoutEvent.basket.CustomerId,
                Status = OrderStatus.Submitted
            };
            foreach (var item in checkoutEvent.basket.Items)
            {
                order.AddOrderItem(item.Id, 1);

            }
            order = await _orderRepository.AddOrderAsync(order);

            var orderStatusChangedToSubmittedIntegrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(checkoutEvent.basket.CustomerId);
            await _eventBus.PublishEvent(orderStatusChangedToSubmittedIntegrationEvent);

        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult> GetOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound();
            }
        }

        private readonly DaprClient _daprClient;

        private readonly IOrderRepository _orderRepository;

        private readonly IEventBus _eventBus;
        private const string DAPR_PUBSUB_NAME = "mydapr-pubsub";

    }
}
