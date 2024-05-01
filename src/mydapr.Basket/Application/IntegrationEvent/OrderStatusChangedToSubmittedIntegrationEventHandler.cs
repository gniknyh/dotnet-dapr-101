using Link.Mydapr.Service.Basket.Infrastucture;

namespace Link.Mydapr.Service.Basket.IntegrationEvent
{
    public class OrderStatusChangedToSubmittedIntegrationEventHandler
    {

        public OrderStatusChangedToSubmittedIntegrationEventHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public Task Handle(OrderStatusChangedToSubmittedIntegrationEvent @event)
        => _basketRepository.RemoveAsync(@event.CustomerId);

        private readonly IBasketRepository _basketRepository;
    }
}