
using Link.Mydapr.Util.Pubsub;

namespace Link.Mydapr.Service.Ordering.Events
{
    public record CheckoutIntegrationEvent(CustomerBasket basket) : IntegrationEvent;
}