using Link.Mydapr.Service.Basket.Infrastucture;

namespace Link.Mydapr.Service.Basket.IntegrationEvent
{
    public class OrderStatusChangedToSubmittedIntegrationEventHandler
    {

        public OrderStatusChangedToSubmittedIntegrationEventHandler(IBasketRepository basketRepository, ILogger<OrderStatusChangedToSubmittedIntegrationEventHandler> logger)
        {
            _basketRepository = basketRepository;
            _logger = logger;
        }

        public async Task Handle(OrderStatusChangedToSubmittedIntegrationEvent @event)
        {
            _logger.LogInformation($"removed basket: {@event.CustomerId}");
            await _basketRepository.RemoveAsync(@event.CustomerId);
        }

        private readonly IBasketRepository _basketRepository;
        private readonly ILogger _logger;
    }
}